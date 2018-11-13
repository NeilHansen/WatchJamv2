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
		




	}

    public void pressed()
    {
        isPressed = true;
        _rend.material = buttonPressed;
        transform.Translate(0, .015f, 0);
    }
     
    public void unPressed()
    {
        isPressed = false;
        _rend.material = buttonNotPressed;
        transform.Translate(0, -.015f, 0);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (isPressed)
            unPressed();
        else if (!isPressed)
            pressed();
    }*/
}
