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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterController>().b_terminalInteraction = false;
        }
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
        GetComponent<bl_MiniMapItem>().SetIconColor(BrokenColor);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
