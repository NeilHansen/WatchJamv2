using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour {

    public float stunTime;
    public GameObject stunnedText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        stunTime -= Time.deltaTime;
		if(stunTime > 0.0f)
        {
            stunnedText.SetActive(true);
            this.GetComponent<PlayerController>().enabled = false;
        }
        else if(stunTime <= 0.0f)
        {
            stunnedText.SetActive(false);
            this.GetComponent<PlayerController>().enabled = true;
        }
	}

   
}
