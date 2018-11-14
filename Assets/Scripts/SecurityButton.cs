using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityButton : MonoBehaviour {

    public bool isPressed;

    public Material buttonPressed;
    public Material buttonNotPressed;

    private MeshRenderer _rend;

	// Use this for initialization
	void Start ()
    {
        isPressed = true;
        _rend = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
        if(isPressed)
        {
            pressed();
        }
        else
        {
            unPressed();
        }



	}

    public void pressed()
    {
        //isPressed = true;
        _rend.material = buttonPressed;
        transform.position = new Vector3 (0.015f, transform.position.y, transform.position.z);
    }
     
    public void unPressed()
    {
       // isPressed = false;
        _rend.material = buttonNotPressed;
        transform.position = new Vector3(-0.015f, transform.position.y, transform.position.z);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (isPressed)
            unPressed();
        else if (!isPressed)
            pressed();
    }*/
}
