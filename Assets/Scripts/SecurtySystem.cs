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
        StartCoroutine("FlashBar");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (alarmIncrease == true || alarmLevel.value > 0.5f) 
        {
            alarmLevel.value = alarmProgress;
            alarmProgress += Time.deltaTime / speed;
            //flash alarm here
            //StartCoroutine("FlashBar");
        }
        else if(!alarmIncrease)
        {
          //  StopAllCoroutines();
        }

        if (alarmLevel.value == alarmLevel.maxValue )
        {

            Debug.Log("You LOOOSSE");
        }
       

       
    }

    private IEnumerator FlashBar()
    {
        yield return new WaitForSeconds(0.25f);
        alarmLevel.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(0.25f);
        alarmLevel.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        StartCoroutine("FlashBar");
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
        StartCoroutine("FlashBar");
    }

    public void alarmOff()
    {
        Debug.Log("STop");
        alarmIncrease = false;
        StopAllCoroutines();
        alarmLevel.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
    }


}
