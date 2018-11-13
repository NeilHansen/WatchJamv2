using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {


    public GameObject[] playerCam;
    private int oldMask;
    private  GameObject hitObject = null;

	// Use this for initialization
	void Start () {
        playerCam = GameObject.FindGameObjectsWithTag("SecurityCam");
        oldMask = playerCam[0].GetComponent<Camera>().cullingMask;
        TurnOffMonsterRender();
        Physics.IgnoreLayerCollision(11, 12, true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            hitObject = other.gameObject;
            for (int i = 0; i < playerCam.Length; i++)
            {
             //   playerCam[i].GetComponent<Camera>().cullingMask = -1;

            }

            other.gameObject.GetComponent<MonsterUIController>().isSeen = true;
            
        }
    }

    public void TurnOffMonsterRender()
    {
       for (int i = 0; i < playerCam.Length; i++)
        {
          //  playerCam[i].GetComponent<Camera>().cullingMask = oldMask;
        }
       if(hitObject != null)
       hitObject.gameObject.GetComponent<MonsterUIController>().isSeen = false;
    }

    public void TurnOnMonsterRender()
    {
        
        for (int i = 0; i < playerCam.Length; i++)
        {
          //  playerCam[i].GetComponent<Camera>().cullingMask = -1;

        }
        if (hitObject != null)
            hitObject.gameObject.GetComponent<MonsterUIController>().isSeen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            for (int i = 0; i < playerCam.Length; i++)
            {
             //   playerCam[i].GetComponent<Camera>().cullingMask = oldMask;

            }
            other.gameObject.GetComponent<MonsterUIController>().isSeen = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
