using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SecurityController : NetworkBehaviour
{
    public NetworkPlayer networkPlayer;
    public Player player;
    public TerminalFixer tFixer;
    public FlashlightController flashLight;
    public SecurityStunned stun;

    [SyncVar(hook = "OnStunChange")]
    public bool b_isStunned = false;
    [SyncVar]
    public bool b_UsingFlashLight = false;
    [SyncVar]
    public bool b_OverHeatFlashLight = false;
    public float flashLightUseTime = 5.0f;
    public float flashLightMaxTime = 5.0f;

    public GameObject terminalInteraction;
    public bool b_terminalInteraction = false;
    public float TerminalFixTime = 3.0f;

    private bl_MiniMap mm;

    // Use this for initialization
    void Start()
    {
        if (hasAuthority)
        {
            //Set MiniMap
            mm = FindObjectOfType<bl_MiniMap>();

            //Set Flashlight variables
            SecurityUI.Instance.SetFlashUIMaxValue(flashLightUseTime);
        }
        else
        {
            GetComponent<bl_MiniMapItem>().enabled = true;
        }

        stun.securityController = this;

        //Give Children References
        tFixer.securityController = this;
        flashLight.securityController = this;
    }

    void Update()
    {
        if (!hasAuthority)
            return;

        if(player.GetButtonDown("MiniMap"))
        {
            mm.ToggleSize();
        }

        //Flashlight network handling
        if (player.GetButton("FlashLight") && flashLightUseTime >= 0.0f && !b_OverHeatFlashLight)
        {
            CmdTurnLightOn();
        }
        else
        {
            CmdTurnLightOff();
            SecurityUI.Instance.SetFlashUIValue(flashLightUseTime);
        }

        //Locally Set flashlight time and UI
        if (b_UsingFlashLight)
        {
            flashLightUseTime -= Time.deltaTime;
            //Set UI
            SecurityUI.Instance.SetFlashUIValue(flashLightUseTime);
        }
        else
        {
            if(flashLightUseTime <= 0.0f && !b_OverHeatFlashLight)
            {
                CmdOverHeartOff();
            }

            //If we used anything then keep refilling it
            if (flashLightUseTime < flashLightMaxTime)
            {
                flashLightUseTime += Time.deltaTime / 2.0f;
            }
            else
            {
                CmdOverHeartOn();
            }

            //Set UI
            SecurityUI.Instance.SetFlashUIValue(flashLightUseTime);
        }

        if(b_terminalInteraction)
        {
            SecurityUI.Instance.TogglePlayerInteractText(true);
        }
        else
        {
            SecurityUI.Instance.TogglePlayerInteractText(false);
        }
    }

    #region Terminal
    //Saveguard so only the server call
    [Command]
    public void CmdSendFixTerminal(GameObject target)
    {
        target.GetComponent<TerminalController>().CmdReceiveFixTerminal();
    }
    #endregion

    #region Flashlight
    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    public void CmdTurnLightOn()
    {
        RpcTurnLightOn();
        flashLight.SecurityFlashLightOn();
    }

    [Command]
    public void CmdTurnLightOff()
    {
        RpcTurnLightOff();
        flashLight.SecurityFlashLightOff();
    }

    //OverHeat
    [Command]
    public void CmdOverHeartOn()
    {
        RpcOverHeartOn();
        b_OverHeatFlashLight = false;
    }

    [Command]
    public void CmdOverHeartOff()
    {
        RpcOverHeartOff();
        b_OverHeatFlashLight = true;
    }

    [ClientRpc]
    public void RpcTurnLightOn()
    {
        flashLight.TurnVioletOn();
    }

    [ClientRpc]
    void RpcTurnLightOff()
    {
        flashLight.TurnVioletOff();
    }

    //OverHeat
    [ClientRpc]
    public void RpcOverHeartOn()
    {
        flashLight.ToggleFlashLightOn();
    }

    [ClientRpc]
    void RpcOverHeartOff()
    {
        flashLight.ToggleFlashLightOff();
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
        stun.ResetStunTimer();
        b_isStunned = true;
    }

    void OnStunChange(bool isStunned)
    {
        b_isStunned = isStunned;

        if (isStunned)
        {
            stun.StunOn();
        }
        else
        {
            stun.StunOff();
        }
    }
    #endregion

}
