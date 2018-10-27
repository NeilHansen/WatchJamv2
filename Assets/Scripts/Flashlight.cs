using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {


    public GameObject[] playerCam;
    private int oldMask;
	// Use this for initialization
	void Start () {
        playerCam = GameObject.FindGameObjectsWithTag("SecurityCam");
        oldMask = playerCam[0].GetComponent<Camera>().cullingMask;
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            for (int i = 0; i < playerCam.Length; i++)
            {
                playerCam[i].GetComponent<Camera>().cullingMask = -1;

            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            for (int i = 0; i < playerCam.Length; i++)
            {
                playerCam[i].GetComponent<Camera>().cullingMask = oldMask;

            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
