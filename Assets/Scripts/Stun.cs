using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour {

    public float stunTime;
    public GameObject stunnedText;

    private Color defaultColor;
	// Use this for initialization
	void Start () {
        defaultColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
        stunTime -= Time.deltaTime;
		if(stunTime > 0.0f)
        {
            stunnedText.SetActive(true);
            this.GetComponent<PlayerController>().enabled = false;
            this.transform.GetChild(0).gameObject.GetComponent<EZCameraShake.CameraShaker>().enabled = true;
            this.gameObject.GetComponent<MeshRenderer>().material.color =  Color.black;
        }
        else if(stunTime <= 0.0f)
        {
            stunnedText.SetActive(false);
            this.GetComponent<PlayerController>().enabled = true;
            //this.transform.GetChild(0).gameObject.GetComponent<EZCameraShake.CameraShaker>().ShakeOnce;
            this.transform.GetChild(0).gameObject.GetComponent<EZCameraShake.CameraShaker>().enabled = false;
            this.gameObject.GetComponent<MeshRenderer>().material.color = defaultColor;
        }
	}

   
}
