using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : MonoBehaviour {

    public bool isBroken;

    public DoorController securitySystem;

	// Use this for initialization
	void Start ()
    {
        securitySystem = GameObject.FindObjectOfType<DoorController>();
            //   isPressed = true;
     //   _rend = GetComponent<MeshRenderer>();
    }


	
	// Update is called once per frame
	void Update ()
    {
	     if(isBroken)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    



	}

    public void pressed()
    {
        //isPressed = true;
       // _rend.material = buttonPressed;
      //  transform.position = new Vector3 (0.015f, transform.position.y, transform.position.z);
    }
     
    public void unPressed()
    {
       // isPressed = false;
      //  _rend.material = buttonNotPressed;
       // transform.position = new Vector3(-0.015f, transform.position.y, transform.position.z);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (isPressed)
            unPressed();
        else if (!isPressed)
            pressed();
    }*/
}
