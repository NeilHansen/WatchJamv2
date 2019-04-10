using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightUIWrapper : MonoBehaviour {

    public SecurityUI s_UI;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(s_UI.flashUI.value >= 5.0f)
        {
            s_UI.dots[0].SetActive(true);
            s_UI.dots[1].SetActive(true);
            s_UI.dots[2].SetActive(true);
            s_UI.dots[3].SetActive(true);
            s_UI.dots[4].SetActive(true);
        }
        else if (s_UI.flashUI.value >= 4.0f && s_UI.flashUI.value <= 5.0f)
        {
            s_UI.dots[0].SetActive(true);
            s_UI.dots[1].SetActive(true);
            s_UI.dots[2].SetActive(true);
            s_UI.dots[3].SetActive(true);
            s_UI.dots[4].SetActive(false);
        }
        else if (s_UI.flashUI.value >= 3.0f && s_UI.flashUI.value <= 4.0f)
        {
            s_UI.dots[0].SetActive(true);
            s_UI.dots[1].SetActive(true);
            s_UI.dots[2].SetActive(true);
            s_UI.dots[3].SetActive(false);
            s_UI.dots[4].SetActive(false);
        }
        else if (s_UI.flashUI.value >= 2.0f && s_UI.flashUI.value <= 3.0f)
        {
            s_UI.dots[0].SetActive(true);
            s_UI.dots[1].SetActive(true);
            s_UI.dots[2].SetActive(false);
            s_UI.dots[3].SetActive(false);
            s_UI.dots[4].SetActive(false);
        }
        else if (s_UI.flashUI.value >= 1.0f && s_UI.flashUI.value <= 2.0f)
        {
            s_UI.dots[0].SetActive(true);
            s_UI.dots[1].SetActive(false);
            s_UI.dots[2].SetActive(false);
            s_UI.dots[3].SetActive(false);
            s_UI.dots[4].SetActive(false);
        }
        else if (s_UI.flashUI.value >= 0.0f && s_UI.flashUI.value <= 1.0f)
        {
            s_UI.dots[0].SetActive(false);
            s_UI.dots[1].SetActive(false);
            s_UI.dots[2].SetActive(false);
            s_UI.dots[3].SetActive(false);
            s_UI.dots[4].SetActive(false);
        }
    }
}
