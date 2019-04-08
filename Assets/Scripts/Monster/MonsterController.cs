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

    private MonsterMovement monsterMovement;

    //Drain and punch Variables
    public float stunTime = 3.0f;
    public float punchLength = 3.0f;
    public float punchCooldownLength = 3.0f;

    public float drainLength = 3.0f;
    public float drainCooldownLength = 3.0f;

    public bool isDrainHitting = false;

    public bool b_terminalInteraction = false;

    public bool isSmashing = false;
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
    public float monsterHealth = 1.0f;
    public float monsterAlphaWhenSeen = 0.35f;
    public float monsterSmashSeenAmount = 1.0f;
    private float oldMonserAlphaWhenSeen;

    public Material monsterMaterial;
    private Color monsterColor;

    private bl_MiniMap mm;
    private Outline outline;

    //Anim Variables
    bool deathAnimPlaying = false;

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

        oldMonserAlphaWhenSeen = monsterAlphaWhenSeen;

        monsterMovement = GetComponent<MonsterMovement>();

        //Give children a reference to this script
        powerDrain.monster = this;
        powerPunch.monster = this;

        //Find Material with monster
        monsterMaterial = GameObject.FindGameObjectWithTag("Monster Material").GetComponent<SkinnedMeshRenderer>().material;
        monsterMaterial.color = monsterColor;

        outline = GetComponent<Outline>();

        MonsterUI.Instance.SetMonsterSeenIcon(false);
        MonsterUI.Instance.SetVisibilitySlider(monsterHealth);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //To Happen on all clients enabled and disabled outline when hurt
        if (Security1DoDamage || Security2DoDamage || Security3DoDamage)
        {
            outline.enabled = true;
        }
        else if (outline.enabled)
        {
            outline.enabled = false;
        }

        //Stuff only for local monster
        if (!hasAuthority)
            return;

        if (player.GetButtonDown("Punch") && b_terminalInteraction)
        {
            isSmashing = true;
            GetComponent<Animator>().SetBool("IsAttacking", true);
        }

        if(isSmashing)
        {
            Debug.Log("Show monster");
            MonsterUI.Instance.SetMonsterSeenIcon(true);
            monsterAlphaWhenSeen = monsterSmashSeenAmount;
            CmdShowMonster();
        }
        else if (!deathAnimPlaying && (Security1InSight || Security2InSight || Security3InSight))
        {
            MonsterUI.Instance.SetMonsterSeenIcon(true);
            monsterAlphaWhenSeen = oldMonserAlphaWhenSeen;
            CmdShowMonster();
        }
        else
        {
            MonsterUI.Instance.SetMonsterSeenIcon(false);
            CmdHideMonster();
        }

        if (!deathAnimPlaying && (Security1DoDamage || Security2DoDamage || Security3DoDamage))
        {
            MonsterUI.Instance.SetMonsterHurt(true);
            CmdTakeDamage();
        }
        else
        {
            MonsterUI.Instance.SetMonsterHurt(false);
        }

        if (monsterHealth <= 0.0f && !deathAnimPlaying)
        {
            TriggerDeath();
        }

        if (player.GetButtonDown("MiniMap"))
        {
            mm.ToggleSize();
        }

        //powerPunch.MonsterPunch();
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
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }
        }
        else
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }
        }
    }

    //Start playing death animation
    public void TriggerDeath()
    {
        if (!hasAuthority)
            return;

        deathAnimPlaying = true;
        monsterAlphaWhenSeen = monsterSmashSeenAmount;
        GetComponent<Animator>().SetBool("DeathAnimPlaying", true);
    }

    //Repspawn player in right position, should only be called by anim events at the end of "Death" anim
    public void ResetMonster()
    {
        if (!hasAuthority)
            return;

        GetComponent<Animator>().SetBool("DeathAnimPlaying", false);
        GetComponent<MonsterMovement>().ResetCameraAfterRespawn();
        deathAnimPlaying = false;
        monsterHealth = 1.0f;
        isSmashing = false;
        b_terminalInteraction = false;
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
        monsterHealth = 1.0f;
    }

    #region Drain and Flashlight
    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    public void CmdTakeDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        if (Security1DoDamage)
            monsterHealth -= damage;
        if (Security2DoDamage)
            monsterHealth -= damage;
        if (Security3DoDamage)
            monsterHealth -= damage;
    }

    [Command]
    public void CmdRemoveDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        monsterHealth += damage;
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
