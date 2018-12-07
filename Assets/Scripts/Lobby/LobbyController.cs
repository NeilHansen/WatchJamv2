using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LobbyController : MonoBehaviour {
    public int playerNumber;
    public int controllerNumber;
    public Player player;

    private bool getNumberOnce = false;

    // Use this for initialization
    void Start () {
        player = Rewired.ReInput.players.GetPlayer(controllerNumber);
    }
	
	// Update is called once per frame
	void Update () {
		if(player.GetButton("Interact") && !getNumberOnce)
        {
            getNumberOnce = true;
            playerNumber = LobbyManager.Instance.playerNumberStack.Pop();
            Debug.Log("Assigning playerNumber " + playerNumber + " to controller " + controllerNumber);
        }
	}
}
