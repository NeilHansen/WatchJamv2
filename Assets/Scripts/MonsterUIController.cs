using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIController : MonoBehaviour {


    public bool isSeen;

    public Slider MonsterUI;

    public float UIValue;
	// Use this for initialization
	void Start () {
        MonsterUI.value = UIValue;

    }
	
	// Update is called once per frame
	void Update () {
       // MonsterUI.value = UIValue;
        if (isSeen)
        {
           
            MonsterUI.value += Time.deltaTime;
        }
        else
        {

            MonsterUI.value -= Time.deltaTime;
        }
        
	}

    public void SwitchDirection()
    {
        if(isSeen)
        {
            isSeen = false;
        }
        else
        {
            isSeen = true;
        }
    }
}
