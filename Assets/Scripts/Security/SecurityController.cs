using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SecurityController : NetworkBehaviour
{
    public Player player;
    public TerminalFixer tFixer;
    public FlashlightController flashLight;
    public SecurityStunned stun;

    public bool b_isInteracting = false;
    [SyncVar(hook = "OnStunChange")]
    public bool b_isStunned = false;
    public float TerminalFixTime = 3.0f;
    public float flashLightMaxTime = 5.0f;

    

    // Use this for initialization
    void Start()
    {
        if (hasAuthority)
        {
            //Set Flashlight variables
            SecurityUI.Instance.SetFlashUIMaxValue(flashLightMaxTime);
        }

        stun.securityController = this;

        flashLight.UseTime = flashLightMaxTime;

        //Give Children References
        tFixer.securityController = this;
        flashLight.securityController = this;
    }

    void Update()
    {
        if(hasAuthority)
        {
            if (player.GetButton("FlashLight"))
            {
                NetworkTurnFlashLightOn();
                SecurityUI.Instance.SetFlashUIValue(flashLight.useTime);
            }
            else
            {
                NetworkTurnFlashLightOff();
                SecurityUI.Instance.SetFlashUIValue(flashLight.useTime);
            }
        }
    }

    void NetworkTurnFlashLightOn()
    {
        if (isServer)
        {
            RpcTurnLightOn();
        }
        else
        {
            flashLight.SecurityFlashLightOn();
            CmdTurnLightOn();
        }
    }

    void NetworkTurnFlashLightOff()
    {
        if (isServer)
        {
            RpcTurnLightOff();
        }
        else
        {
            flashLight.SecurityFlashLightOff();
            CmdTurnLightOff();
        }
    }

    //This is a Network command, so the damage is done to the relevant GameObject
    [ClientRpc]
    void RpcTurnLightOn()
    {
        flashLight.SecurityFlashLightOn();
    }

    [ClientRpc]
    void RpcTurnLightOff()
    {
        flashLight.SecurityFlashLightOff();
    }

    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    void CmdTurnLightOn()
    {
        flashLight.SecurityFlashLightOn();
    }

    [Command]
    void CmdTurnLightOff()
    {
        flashLight.SecurityFlashLightOff();
    }

    #region Monster Punch
    //Command to receive when being punched
    [Command]
    public void CmdReceivePunch()
    {
        RpcPunchEffect();
        stun.ResetStunTimer();
        b_isStunned = true;
    }

    [ClientRpc]
    void RpcPunchEffect()
    {
        //Delete this function if you don't need it.
    }

    void OnStunChange(bool isStunned)
    {
        if (isStunned)
            stun.StunOn();
        else
            stun.StunOff();
    }
    #endregion

}
