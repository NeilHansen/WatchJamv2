﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MonsterController : NetworkBehaviour {
    public NetworkPlayer networkPlayer;
    public Player player;

    public PowerDrain powerDrain;
    public PowerPunch powerPunch;

    //Drain and punch Variables
    public float stunTime = 3.0f;
    public float punchLength = 3.0f;
    public float punchCooldownLength = 3.0f;

    public float drainLength = 3.0f;
    public float drainCooldownLength = 3.0f;

    public bool isFlashLightHitting = false;
    public bool isDrainHitting = false;

    public bool b_terminalInteraction = false;

    public bool isDraining = false;
    public bool drainCooldown = false;
    public bool isPunching = false;
    public bool punchCooldown = false;

    //Monster Transparency
    public float materialAlphaChangeRate = 0.1f;
    [SyncVar(hook = "OnChangeMonsterAlpha")]
    public float currentAlpha = 0.0f;

    private Material monsterMaterial;
    private Color monsterColor;


    private bl_MiniMap mm;

    // Use this for initialization
    void Start () {
        if(hasAuthority)
        {
            //Set MiniMap
            mm = FindObjectOfType<bl_MiniMap>();
        }

        //Give children a reference to this script
        powerDrain.monster = this;
        powerPunch.monster = this;

        //Find Material with monster
        monsterMaterial = GameObject.FindGameObjectWithTag("Monster Material").GetComponent<SkinnedMeshRenderer>().material;
        monsterMaterial.color = monsterColor;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!hasAuthority)
            return;

        if (player.GetButtonDown("MiniMap"))
        {
            mm.ToggleSize();
        }

        if (currentAlpha >= 1.0f)
        {
            GameManager.Instance.ResetMonster();
        }

        powerPunch.MonsterPunch();
        powerDrain.MonsterDrain();

        if (b_terminalInteraction)
        {
            MonsterUI.Instance.ToggleMonsterInteractText(true);
        }
        else
        {
            MonsterUI.Instance.ToggleMonsterInteractText(false);
        }
    }

    public void TerminalInteractionOn()
    {
        SecurityUI.Instance.TogglePlayerInteractText(true);
    }

    public void TerminalInteractionOff()
    {
        SecurityUI.Instance.TogglePlayerInteractText(false);
    }

    void OnChangeMonsterAlpha(float alpha)
    {
        //Bug current alpha doesn't replicate across
        currentAlpha = alpha;

        if (alpha > 0.0)
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetMonsterSeenIcon(true);
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }

            monsterColor.a = currentAlpha;
            monsterMaterial.color = monsterColor;
        }
        else
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetMonsterSeenIcon(false);
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }

            monsterColor.a = 0;
            monsterMaterial.color = monsterColor;
        }
    }

    //For reseting the monster
    [Command]
    public void CmdResetAlpha()
    {
        RpcResetAlpha();
    }

    [ClientRpc]
    public void RpcResetAlpha()
    {
        currentAlpha = 0.0f;
    }

    #region Drain and Flashlight
    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    public void CmdTakeDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        currentAlpha += damage;
    }

    [Command]
    public void CmdRemoveDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        currentAlpha -= damage;
    }
    #endregion

    #region Monster Punch
    //Saveguard so only the server call
    [Command]
    public void CmdStunTarget(GameObject target)
    {
        target.GetComponent<SecurityController>().CmdReceivePunch();
    }

    //Saveguard so only the server call
    [Command]
    public void CmdSendBreakTerminal(GameObject target)
    {
        target.GetComponent<TerminalController>().CmdReceiveBreakTerminal();
    }
    #endregion
}
