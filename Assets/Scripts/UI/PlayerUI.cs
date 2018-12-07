using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public int playerNumber;

    public Text interact;
    public Text stunnedText;

    public Slider flashUI;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitUI(int playerN)
    {
        playerNumber = playerN;

        //Set display to the correct player number
        GetComponent<Canvas>().targetDisplay = playerNumber;

        //Add to UIManager dictionary
        UIManager.Instance.playerUIsDictionary.Add(playerNumber, this);
    }

    public void SetFlashUIValue(float value)
    {
        flashUI.value = value;
    }

    public void SetFlashUIMaxValue(float value)
    {
        flashUI.maxValue = value;
    }

    public void TogglePlayerInteractText(bool b)
    {
        interact.enabled = b;
    }

    public void ToggleStunnedText(bool b)
    {
        stunnedText.enabled = b;
    }
}
