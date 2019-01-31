using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SecurityStunned : NetworkBehaviour {
    public SecurityController securityController;

    public float stunTime = 3.0f;
    private float defaultStunTime;

    private MeshRenderer meshRend;
    private Color defaultColor;

    // Use this for initialization
    void Start()
    {
        defaultStunTime = stunTime;
        meshRend = GetComponent<MeshRenderer>();
        defaultColor = meshRend.material.color;
    }

    void Update()
    {
        if(hasAuthority)
        {
            if (securityController.b_isStunned)
            {
                stunTime -= Time.deltaTime;
                if(stunTime <= 0.0)
                {
                    securityController.b_isStunned = false;
                    stunTime = defaultStunTime;
                    SecurityUI.Instance.ToggleStunnedText(false);
                    NetworkStunOff();
                }
                else
                {
                    NetworkStunOn();
                    SecurityUI.Instance.ToggleStunnedText(true);
                }
            }
        }
    }

    void NetworkStunOn()
    {
        if (isServer)
        {
            RpcStunOn();
        }
        else
        {
            RpcStunOff();
        }
    }

    void NetworkStunOff()
    {
        if (isServer)
        {
            CmdStunOn();
        }
        else
        {
            CmdStunOff();
        }
    }

    //This is a Network command, so the damage is done to the relevant GameObject
    [ClientRpc]
    void RpcStunOn()
    {
        StunOn();
    }

    [ClientRpc]
    void RpcStunOff()
    {
        StunOff();
    }

    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    void CmdStunOn()
    {
        StunOn();
    }

    [Command]
    void CmdStunOff()
    {
        StunOff();
    }

    void StunOn()
    {
        securityController.enabled = true;
        meshRend.material.color = defaultColor;
    }

    void StunOff()
    {
        securityController.enabled = false;
        meshRend.material.color = Color.black;
    }
}
