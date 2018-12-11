using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TerminalFixer : MonoBehaviour {

    public PlayerController playerController;

    private float heldTime = 0.0f;
    private TerminalController hitObject;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //If pressing key and is interacting with terminal
        if (playerController.player.GetButton("Interact") && playerController.b_isInteracting)
        {
            heldTime += Time.deltaTime;
            
            if (hitObject.isBroken == true)
            {
                if (heldTime >= playerController.TerminalFixTime)
                {
                    hitObject.isBroken = false;
                    DoorController.Instance.CheckDoors();
                    hitObject.gameObject.GetComponent<MapBlip>().color = Color.white;
                }
            }
            else
            {
                if (playerController.flashLightMaxTime < 5)
                {
                    playerController.flashLightMaxTime += Time.deltaTime;
                }
            }
        }

        //If let go reset Timer
        if (playerController.player.GetButtonUp("Interact"))
        {
            heldTime = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            hitObject = other.GetComponent<TerminalController>();
            playerController.b_isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            hitObject = null;
            playerController.b_isInteracting = false;
        }
    }
}
