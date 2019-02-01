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
        if (!hasAuthority)
            return;

        if (player.GetButton("FlashLight"))
        {
            CmdTurnLightOn();
            SecurityUI.Instance.SetFlashUIValue(flashLight.useTime);
        }
        else
        {
            CmdTurnLightOff();
            SecurityUI.Instance.SetFlashUIValue(flashLight.useTime);
        }
    }

    #region Flashlight
    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    public void CmdTurnLightOn()
    {
        RpcTurnLightOn();
        flashLight.SecurityFlashLightOn();
    }

    [ClientRpc]
    public void RpcTurnLightOn()
    {
        flashLight.TurnVioletOn();
    }

    [Command]
    public void CmdTurnLightOff()
    {
        RpcTurnLightOff();
        flashLight.SecurityFlashLightOff();
    }

    [ClientRpc]
    void RpcTurnLightOff()
    {
        flashLight.TurnVioletOff();
    }

    //Make sure that target only takes damage
    [Command]
    public void CmdDamageTarget(GameObject target)
    {
        target.GetComponent<MonsterController>().CmdTakeDamage();
    }
    #endregion

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
