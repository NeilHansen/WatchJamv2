using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunned : MonoBehaviour {
    public PlayerController playerController;

    private MeshRenderer meshRend;
    private Color defaultColor;

	// Use this for initialization
	void Start () {
        meshRend = GetComponent<MeshRenderer>();
        defaultColor = meshRend.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        playerController.stunTime -= Time.deltaTime;
		if(!(playerController.stunTime <= 0.0f))
        {
            UIManager.Instance.ToggleStunnedText(playerController.playerNumber, true);
            playerController.enabled = false;
            //this.transform.GetChild(0).gameObject.GetComponent<ScreenShaker>().ShakeIt();
            meshRend.material.color =  Color.black;
        }
        else
        {
            UIManager.Instance.ToggleStunnedText(playerController.playerNumber, false);
            playerController.enabled = true;
            meshRend.material.color = defaultColor;
        }
	}  
}
