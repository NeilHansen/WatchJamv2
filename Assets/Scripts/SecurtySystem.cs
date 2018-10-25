using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurtySystem : MonoBehaviour {


    [SerializeField] Slider alarmLevel;

    private float alarmProgress;
    private bool alarmIncrease;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(alarmIncrease == true)
        {
            alarmProgress += Time.deltaTime / 2;
        }

        if (alarmLevel.value > .5)
        {
            startTimer();
            alarmProgress += Time.deltaTime;
        }
    }

    public void valueChange()
    {
        Debug.Log("changing");

    }

    private void startTimer()
    {
        Debug.Log("Incoming You Suck");
    }

    /*public void alarmOn()
    {
        alarmIncrease = true;
    }

    public void alarmOff()
    {
        alarmIncrease = false;
    }*/


}
