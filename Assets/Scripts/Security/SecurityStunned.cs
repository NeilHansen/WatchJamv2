﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SecurityStunned : NetworkBehaviour {
    public SecurityController securityController;

    private float stunTimer = 0.0f;
    public float stunDuration = 5.0f;
    private MeshRenderer meshRend;
    private Color defaultColor;

    private UGUIMiniMap.bl_FirstPersonControllerSecurity movement;

    // Use this for initialization
    void Start()
    {
        movement = GetComponent<UGUIMiniMap.bl_FirstPersonControllerSecurity>();

        meshRend = GetComponent<MeshRenderer>();
        defaultColor = meshRend.material.color;
    }

    void Update()
    {
        if (isServer)
        {
            if (securityController.b_isStunned)
            {
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0.0)
                {
                    securityController.b_isStunned = false;
                }
            }
        }
    }

    public void StunOn()
    {
        if(hasAuthority)
        {
            SecurityUI.Instance.ToggleStunnedText(true);
        }

        movement.enabled = false;
        securityController.enabled = false;
        meshRend.material.color = Color.black;
    }

    public void StunOff()
    {
        if (hasAuthority)
        {
            SecurityUI.Instance.ToggleStunnedText(false);
        }

        movement.enabled = true;
        securityController.enabled = true;
        meshRend.material.color = defaultColor;
    }

    public void ResetStunTimer()
    {
        stunTimer = stunDuration;
    }
}
