using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TerminalFixer : MonoBehaviour {

    public PlayerController playerController;

    public float heldTime = 0.0f;
    public bool isInteracting = false;

    private TerminalController hitObject;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //If pressing key and is interacting with terminal
        if (playerController.player.GetButton("Interact") && isInteracting)
        {
            heldTime += Time.deltaTime;
            
            if (hitObject.isBroken == true)
            {
                if (heldTime >= 3.0f)
                {
                    hitObject.isBroken = false;
                    DoorController.Instance.CheckDoors();
                    hitObject.gameObject.GetComponent<MapBlip>().color = Color.white;
                }
            }
            else
            {
                if (playerController.flashLight.maxTime < 5)
                {
                    playerController.flashLight.maxTime += Time.deltaTime;
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
            isInteracting = true;
            UIManager.Instance.ToggleInteractText(playerController.playerNumber, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            hitObject = null;
            isInteracting = false;
            UIManager.Instance.ToggleInteractText(playerController.playerNumber, false);
        }
    }
}
