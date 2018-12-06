using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPunch : MonoBehaviour {

    public MonsterController monster;

    public float stunTime;

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
            other.gameObject.GetComponent<Stun>().stunTime = stunTime;
            Debug.Log(other.gameObject.name + " Stunned");
        }

        if(other.gameObject.tag == "Terminal")
        {
            other.gameObject.GetComponent<TerminalController>().isBroken = true;
            other.gameObject.GetComponent<TerminalController>().securitySystem.CheckDoors();
            other.gameObject.GetComponent<MapBlip>().color = Color.red;
            other.gameObject.GetComponent<MapBlipMonster>().color = Color.red;
        }
    }

    void MonsterPunch()
    {
        //Monster punch
        if (monster.player.GetButtonDown("Punch") && !monster.isPunching)
        {
            monster.isPunching = true;

            //To stop punch
            StartCoroutine(stopPunching());
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


    //To stop punching
    IEnumerator stopPunching()
    {
        //Keep color semi - transparent
        UIManager.Instance.punchColor.a = 0.35f;
        UIManager.Instance.punchIcon.color = UIManager.Instance.punchColor;

        yield return new WaitForSeconds(monster.punchLength);

        //Keep transparent when on cooldown
        UIManager.Instance.punchColor.a = 0.35f;
        UIManager.Instance.punchIcon.color = UIManager.Instance.punchColor;

        monster.punchCooldown = true;

        yield return new WaitForSeconds(monster.punchCooldownLength);

        //Turn off
        UIManager.Instance.punchColor.a = 1.0f;
        UIManager.Instance.punchIcon.color = UIManager.Instance.punchColor;

        monster.isPunching = false;
        monster.punchCooldown = false;
    }

}
