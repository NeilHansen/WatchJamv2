using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Networking;

public class TerminalFixer : NetworkBehaviour {

    public SecurityController securityController;

    private float heldTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
        if(hasAuthority)
        {
            //If pressing key and is interacting with terminal
            if (securityController.player.GetButton("Interact") && securityController.b_terminalInteraction && !securityController.b_isStunned)
            {
                heldTime += Time.deltaTime;

                if (heldTime >= securityController.TerminalFixTime)
                {
                    securityController.CmdSendFixTerminal(securityController.terminalInteraction);
                }
            }
            else
            {
                heldTime = 0.0f;
            }
        }
    }
}
