using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TerminalFixer : MonoBehaviour {

    public SecurityController securityController;

    private float heldTime = 0.0f;
    private TerminalController hitObject;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //If pressing key and is interacting with terminal
        if (securityController.player.GetButton("Interact") && securityController.b_isInteracting)
        {
            heldTime += Time.deltaTime;
            
            if (hitObject.isBroken == true)
            {
                if (heldTime >= securityController.TerminalFixTime)
                {
                    hitObject.isBroken = false;
                    DoorController.Instance.CheckDoors();
                }
            }
            else
            {
                if (securityController.flashLightMaxTime < 5)
                {
                    securityController.flashLightMaxTime += Time.deltaTime;
                }
            }
        }

        //If let go reset Timer
        if (securityController.player.GetButtonUp("Interact"))
        {
            heldTime = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            hitObject = other.GetComponent<TerminalController>();
            securityController.b_isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            hitObject = null;
            securityController.b_isInteracting = false;
        }
    }
}
