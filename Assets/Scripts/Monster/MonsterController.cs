using System.Collections;
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

    public bool isDrainHitting = false;

    public bool b_terminalInteraction = false;

    public bool isDraining = false;
    public bool drainCooldown = false;
    public bool isPunching = false;
    public bool punchCooldown = false;

    [SyncVar]
    public bool Security1InSight = false;
    [SyncVar]
    public bool Security2InSight = false;
    [SyncVar]
    public bool Security3InSight = false;

    [SyncVar]
    public bool Security1DoDamage = false;
    [SyncVar]
    public bool Security2DoDamage = false;
    [SyncVar]
    public bool Security3DoDamage = false;

    //Monster Transparency
    public float materialAlphaChangeRate = 0.1f;
    [SyncVar(hook = "OnChangeMonsterAlpha")]
    public float monsterHealth = 0.1f;
    public float monsterAlphaWhenSeen = 0.35f;

    public Material monsterMaterial;
    private Color monsterColor;

    private bl_MiniMap mm;

    // Use this for initialization
    void Start () {
        if(hasAuthority)
        {
  
            mm = FindObjectOfType<bl_MiniMap>();
            //Set MiniMap off
           // mm.gameObject.transform.parent.gameObject.SetActive(false);

            TerminalController[] terminalControllers = FindObjectsOfType<TerminalController>();
            foreach(TerminalController t in terminalControllers)
            {
                t.ShowOutline(true);
            }
        }

        //Give children a reference to this script
        powerDrain.monster = this;
        powerPunch.monster = this;

        //Find Material with monster
        monsterMaterial = GameObject.FindGameObjectWithTag("Monster Material").GetComponent<SkinnedMeshRenderer>().material;
        monsterMaterial.color = monsterColor;

        MonsterUI.Instance.SetMonsterSeenIcon(false);
        MonsterUI.Instance.SetVisibilitySlider(monsterHealth);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!hasAuthority)
            return;

        if (Security1InSight || Security2InSight || Security3InSight)
        {
            CmdShowMonster();
        }
        else
        {
            CmdHideMonster();
        }

        if (Security1DoDamage || Security2DoDamage || Security3DoDamage)
        {
            CmdTakeDamage();
        }

        if (monsterHealth >= 1.0f)
        {
            monsterHealth = 0.0f;
            ResetMonster();
        }

        if (player.GetButtonDown("MiniMap"))
        {
            mm.ToggleSize();
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
        monsterHealth = alpha;

        if (alpha > 0.0)
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetMonsterSeenIcon(true);
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }
        }
        else
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetMonsterSeenIcon(false);
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }
        }
    }

    //Repspawn player in right position
    public void ResetMonster()
    {
        CmdMinusMonsterLife();
        CmdResetHealth();
        MonsterUI.Instance.ResetMonsterUI();
        transform.position = GameManager.Instance.GetMonsterSpawnPosition().position;
        Debug.Log("Respawn Mon");
    }

    //For reseting the monster
    [Command]
    public void CmdMinusMonsterLife()
    {
        GameManager.Instance.MinusMonsterLife();
    }

    //For reseting the monster
    [Command]
    public void CmdResetHealth()
    {
        monsterHealth = 0.0f;
    }

    #region Drain and Flashlight
    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    public void CmdTakeDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        monsterHealth += damage;
    }

    [Command]
    public void CmdRemoveDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        monsterHealth -= damage;
    }

    [Command]
    public void CmdSecurityDamage(int playerNumber, bool b)
    {
        switch (playerNumber)
        {
            case 1:
                Security1DoDamage = b;
                break;
            case 2:
                Security2DoDamage = b;
                break;
            case 3:
                Security3DoDamage = b;
                break;
        }
    }

    public bool CheckSecurityDamage(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return Security1DoDamage;
            case 2:
                return Security2DoDamage;
            case 3:
                return Security3DoDamage;

            default:
                return false;
        }
    }

    [Command]
    public void CmdShowMonster()
    {
        RpcShowMonster();
    }

    [Command]
    public void CmdHideMonster()
    {
        RpcHideMonster();
    }

    [ClientRpc]
    public void RpcShowMonster()
    {
        monsterColor.a = monsterAlphaWhenSeen;
        monsterMaterial.color = monsterColor;
    }

    [ClientRpc]
    public void RpcHideMonster()
    {
        monsterColor.a = 0;
        monsterMaterial.color = monsterColor;
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
        //count up number of broken Terminals
        GameManager.Instance.AddToTermainalCount();
    }
    #endregion
}
