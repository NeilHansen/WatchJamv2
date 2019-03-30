using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerminalController : NetworkBehaviour {

    [SyncVar]
    public bool isBroken = false;

    public Color WorkingColor;
    public Color BrokenColor;

    public Outline terminal1;
    private MonsterController monster;

    public GameObject Position;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            monster = other.GetComponent<MonsterController>();
            monster.b_terminalInteraction = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            monster = other.GetComponent<MonsterController>();
            if(monster.isSmashing)
            {
                other.transform.position = Position.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            monster = other.GetComponent<MonsterController>();
            monster.b_terminalInteraction = false;
        }
    }

    public void ShowOutline(bool b)
    {
        terminal1.enabled = b;
    }

    [Command]
    public void CmdReceiveBreakTerminal()
    {
        isBroken = true;
        RpcBreakTerminal();
    }

    [ClientRpc]
    void RpcBreakTerminal()
    {
        DoorController.Instance.CheckDoors();
        monster.isSmashing = false;
        monster.b_terminalInteraction = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<bl_MiniMapItem>().SetIconColor(BrokenColor);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
