using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterBreakTerminal : NetworkBehaviour
{
    public GameObject leftHandLocation;

    void SmashTerminal()
    {
        if(hasAuthority)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].transform.tag == "Terminal" && !hitColliders[i].transform.GetComponent<TerminalController>().isBroken)
                {
                    GetComponent<Animator>().SetBool("IsAttacking", false);
                    hitColliders[i].transform.GetComponent<TerminalController>().CmdReceiveBreakTerminal();
                }
                i++;
            }
        }
    }
}
