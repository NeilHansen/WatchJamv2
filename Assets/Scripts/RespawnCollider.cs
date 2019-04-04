using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class RespawnCollider : MonoBehaviour {

    public GameObject Sp;
    public int controllerNumber = 0;
    public Player player;

    // Use this for initialization
    void Start () {
        player = Rewired.ReInput.players.GetPlayer(controllerNumber);
    }
	
	// Update is called once per frame
	void Update () {
		if(player.GetButtonDown("Respawn"))
        {
            GameManager.Instance.localPlayer.gameObject.transform.position = Sp.gameObject.transform.position;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = Sp.gameObject.transform.position;
    }

}
