using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public int playerNumber;

    public Text interact;

    // Use this for initialization
    void Start () {
        //Set display to the correct player number
        GetComponent<Canvas>().targetDisplay = playerNumber;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleInteractText(bool b)
    {
        interact.enabled = b;
    }
}
