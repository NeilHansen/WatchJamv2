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
        alarmLevel.value = alarmProgress;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            alarmProgress += 0.25f;
        }

        if (alarmIncrease == true)
        {
            alarmProgress += Time.deltaTime / 30;
        }

        if (alarmLevel.value > .5)
        {
            startTimer();
            alarmProgress += Time.deltaTime / 40;
        }
    }

    /*public void valueChange()
    {
        Debug.Log("changing");

    }*/

    private void startTimer()
    {
        Debug.Log("Incoming You Suck");
    }

    public void alarmOn()
    {
        Debug.Log("should be moving");
        alarmIncrease = true;
    }

    public void alarmOff()
    {
        Debug.Log("should be moving");
        alarmIncrease = false;
    }


}
