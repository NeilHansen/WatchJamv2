using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPunch : MonoBehaviour {
    public MonsterController monster;

    private BoxCollider boxCollider;

    // Use this for initialization
    void Start () {
        boxCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        MonsterPunch();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Security")
        {
            other.gameObject.GetComponent<PlayerController>().stunTime = monster.stunTime;
            Debug.Log(other.gameObject.name + " Stunned");
        }

        if(other.gameObject.tag == "Terminal")
        {

            other.gameObject.GetComponent<TerminalController>().isBroken = true;
            DoorController.Instance.CheckDoors();
            other.gameObject.GetComponent<MapBlip>().color = Color.red;
        }
    }

    void MonsterPunch()
    {
        //Monster punch
        if (monster.player.GetButtonDown("Punch") && !monster.isPunching)
        {
            monster.isPunching = true;

            //To stop punch
            UIManager.Instance.stopPunching(monster.playerNumber, monster);
        }
        //Is in punching mode
        else if (monster.isPunching && !monster.punchCooldown)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
    }
}
