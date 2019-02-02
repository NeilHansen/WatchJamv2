using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPunch : MonoBehaviour {
    public MonsterController monster;

    public BoxCollider boxCollider;
    public MeshRenderer meshRender;

    // Use this for initialization
    void Start () {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Security")
        {
            //Cannot call security commands if not server, have to delegate stun target to server.
            monster.CmdStunTarget(other.gameObject);
        }

        if(other.gameObject.tag == "Terminal")
        {
            monster.CmdSendBreakTerminal(other.gameObject);           
        }
    }

    public void MonsterPunch()
    {
        //Monster punch
        if (monster.player.GetButtonDown("Punch") && !monster.isPunching)
        {
            monster.isPunching = true;
            meshRender.enabled = true;

            //To stop punch
            StartCoroutine(stopPunching());
        }
        //Is in punching mode
        else if (monster.isPunching && !monster.punchCooldown)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
            meshRender.enabled = false;
        }
    }

    //To stop punching
    private IEnumerator stopPunching()
    {
        //Keep color semi - transparent
        MonsterUI.Instance.SetPunchIcon();

        yield return new WaitForSeconds(monster.punchLength);

        //Keep transparent when on cooldown
        MonsterUI.Instance.SetPunchIcon();

        monster.punchCooldown = true;

        yield return new WaitForSeconds(monster.punchCooldownLength);

        //Turn off
        MonsterUI.Instance.SetPunchIcon(1.0f);

        monster.isPunching = false;
        monster.punchCooldown = false;
    }
}
