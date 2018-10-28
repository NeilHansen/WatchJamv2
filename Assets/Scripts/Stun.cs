using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour {

    public float stunTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        stunTime -= Time.deltaTime;
		if(stunTime > 0.0f)
        {
            this.GetComponent<PlayerController>().enabled = false;
        }
        else if(stunTime <= 0.0f)
        {
            this.GetComponent<PlayerController>().enabled = true;
        }
	}

   
}
