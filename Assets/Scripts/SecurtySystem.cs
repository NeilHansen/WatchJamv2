using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurtySystem : MonoBehaviour {


    private Slider alarmLevel;

    private float alarmProgress;
    public bool alarmIncrease;

    public float speed;
    public float reinformentSpeed;

	// Use this for initialization
	void Start ()
    {
       alarmLevel = this.transform.GetChild(0).GetComponent<Slider>();
        alarmIncrease = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (alarmIncrease == true || alarmLevel.value > 0.5f) 
        {
            alarmLevel.value = alarmProgress;
            alarmProgress += Time.deltaTime / speed;
        }

        if (alarmLevel.value == alarmLevel.maxValue )
        {

            Debug.Log("You LOOOSSE");
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
        Debug.Log("should be moving UP");
        alarmIncrease = true;
    }

    public void alarmOff()
    {
        Debug.Log("STop");
        alarmIncrease = false;
    }


}
