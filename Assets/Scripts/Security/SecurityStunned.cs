using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SecurityStunned : NetworkBehaviour {
    public SecurityController securityController;

    private MeshRenderer meshRend;
    private Color defaultColor;

	// Use this for initialization
	void Start () {
        meshRend = GetComponent<MeshRenderer>();
        defaultColor = meshRend.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        if(hasAuthority)
        {
            securityController.stunTime -= Time.deltaTime;
            if (!(securityController.stunTime <= 0.0f))
            {
                SecurityUI.Instance.ToggleStunnedText(true);
                securityController.enabled = false;
                //this.transform.GetChild(0).gameObject.GetComponent<ScreenShaker>().ShakeIt();
                meshRend.material.color = Color.black;
            }
            else
            {
                SecurityUI.Instance.ToggleStunnedText(false);
                securityController.enabled = true;
                meshRend.material.color = defaultColor;
            }
        }
    }  
}
