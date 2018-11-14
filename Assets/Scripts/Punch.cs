using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {

    public float stunTime;
  
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Security")
        {
            other.gameObject.GetComponent<Stun>().stunTime = stunTime;
            Debug.Log(other.gameObject.name + " Stunned");
        }

        if(other.gameObject.tag == "Terminal")
        {
            other.gameObject.GetComponent<TerminalController>().isBroken = true;
            other.gameObject.GetComponent<TerminalController>().securitySystem.CheckDoors();
        }
    }


    
}
