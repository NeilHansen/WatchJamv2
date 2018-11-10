using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityButton : MonoBehaviour {

    public bool isPressed;
    //public Color buttonColor;


	// Use this for initialization
	void Start ()
    {
        isPressed = true;
        //gameObject.GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update ()
    {
		




	}

    public void pressed()
    {
        isPressed = true;
        
    }

    public void unPressed()
    {
        isPressed = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPressed)
            unPressed();
        else if (!isPressed)
            pressed();
    }
}
