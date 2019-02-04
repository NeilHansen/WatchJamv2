using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerminalController : NetworkBehaviour {

    [SyncVar]
    public bool isBroken = false;

    public Color WorkingColor;
    public Color BrokenColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterController>().b_terminalInteraction = true;
        }

        if (other.gameObject.tag == "Security")
        {
            if(isBroken)
            {
                other.GetComponent<SecurityController>().b_terminalInteraction = true;
                other.GetComponent<SecurityController>().terminalInteraction = gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterController>().b_terminalInteraction = false;
        }

        if (other.gameObject.tag == "Security")
        {
            other.GetComponent<SecurityController>().b_terminalInteraction = false;
        }
    }

    [Command]
    public void CmdReceiveBreakTerminal()
    {
        isBroken = true;
        RpcBreakTerminal();
    }

    [Command]
    public void CmdReceiveFixTerminal()
    {
        isBroken = false;
        RpcFixTerminal();
    }

    [ClientRpc]
    void RpcBreakTerminal()
    {
        DoorController.Instance.CheckDoors();
        GetComponent<bl_MiniMapItem>().SetIconColor(BrokenColor);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    [ClientRpc]
    void RpcFixTerminal()
    {
        DoorController.Instance.CheckDoors();
        GetComponent<bl_MiniMapItem>().SetIconColor(WorkingColor);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
