﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class SecurityController : NetworkBehaviour
{
    public NetworkPlayer networkPlayer;
    [SyncVar]
    public string playername = "NoName";
    public Player player;
    public FlashlightController flashLight;
    public SecurityStunned stun;

    public TMP_Text playerName;

    [SyncVar(hook = "OnStunChange")]
    public bool b_isStunned = false;
    [SyncVar]
    public bool b_UsingFlashLight = false;
    [SyncVar]
    public bool b_OverHeatFlashLight = false;
    public float flashLightUseTime = 5.0f;
    public float flashLightMaxTime = 5.0f;

    public GameObject pingIcon;
    public bool isPinging = false;
    public float pingTime = 3.0f;
    public Outline playerOutline;

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

        if(isServer)
            playername = networkPlayer.playerName;

        stun.securityController = this;

        //Give Children References
        flashLight.securityController = this;
    }

    void Update()
    {
        playerName.text = playername;

        if (!hasAuthority)
            return;

        if (!isPinging && player.GetButtonDown("Ping"))
        {
            isPinging = true;
            CmdSecurityPing();
        }

        if (player.GetButtonDown("MiniMap"))
        {
            mm.ToggleSize();
        }

        //Flashlight UV network handling
        if (player.GetButton("FlashLight") && flashLightUseTime >= 0.0f && !b_OverHeatFlashLight)
        {
            CmdTurnUVLightOn();
        }
        else
        {
            CmdTurnUVLightOff();
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
                SecurityUI.Instance.SetOverHeatIcon(true);
                CmdOverHeatOff();
            }
            //If we used anything then keep refilling it
            if (flashLightUseTime < flashLightMaxTime)
            {
                SecurityUI.Instance.SetOverHeatIcon(true);
                flashLightUseTime += Time.deltaTime / 2.0f;
            }
            else
            {
                SecurityUI.Instance.SetOverHeatIcon(false);
                CmdOverHeatOn();
            }

            //Set UI
            SecurityUI.Instance.SetFlashUIValue(flashLightUseTime);
        }
    }

    [Command]
    public void CmdSecurityPing()
    {
        RpcSecurityPing();
        if (!GameManager.Instance.isMonster)
        {
            StartCoroutine(InstatiatePingIcon());
        }
    }

    [ClientRpc]
    public void RpcSecurityPing()
    {
        if (!GameManager.Instance.isMonster)
        {
            StartCoroutine(InstatiatePingIcon());
        }
    }

    private IEnumerator InstatiatePingIcon()
    {
        playerOutline.enabled = true;
        GameObject temp = Instantiate(pingIcon, this.gameObject.transform);
        yield return new WaitForSeconds(pingTime);
        playerOutline.enabled = false;
        isPinging = false;
        Destroy(temp);
    }

    #region Flashlight
    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    public void CmdTurnUVLightOn()
    {
        RpcTurnUVLightOn();
        flashLight.SecurityUVFlashLightOn();
    }

    [Command]
    public void CmdTurnUVLightOff()
    {
        RpcTurnUVLightOff();
        flashLight.SecurityUVFlashLightOff();
    }

    //OverHeat
    [Command]
    public void CmdOverHeatOn()
    {
        RpcOverHeatOn();
        b_OverHeatFlashLight = false;
    }

    [Command]
    public void CmdOverHeatOff()
    {
        RpcOverHeatOff();
        b_OverHeatFlashLight = true;
    }

    [ClientRpc]
    public void RpcTurnUVLightOn()
    {
        flashLight.TurnUVOn();
    }

    [ClientRpc]
    void RpcTurnUVLightOff()
    {
        flashLight.TurnUVOff();
    }

    //OverHeat
    [ClientRpc]
    public void RpcOverHeatOn()
    {
        flashLight.ToggleFlashLightOn();
    }

    [ClientRpc]
    void RpcOverHeatOff()
    {
        flashLight.ToggleFlashLightOff();
    }

    //Make sure that target only takes damage
    [Command]
    public void CmdDamageTarget(GameObject target, int playerNumber, bool b)
    {
        target.GetComponent<MonsterController>().CmdSecurityDamage(playerNumber, b);
    }

    //Make sure that target only takes damage
    [Command]
    public void CmdShowMonster(GameObject target)
    {
        target.GetComponent<MonsterController>().CmdShowMonster();
    }

    [Command]
    public void CmdHideMonster(GameObject target)
    {
        target.GetComponent<MonsterController>().CmdHideMonster();
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
