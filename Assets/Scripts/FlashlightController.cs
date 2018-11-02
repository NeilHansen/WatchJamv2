using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour {

    public GameObject flashlight;
    public GameObject lense;

    public bool shine;

    public float maxTime;
    public float useTime;
    public float rechargeRate;

    public Slider flashUI;

    public float brightness;

	// Use this for initialization
	void Start () {
        flashUI.maxValue = maxTime;
        useTime = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
        flashUI.value = useTime;
		if(shine == true)
        {
            if(useTime > 0.0f)
            {
                useTime -= Time.deltaTime;
                flashlight.GetComponent<Light>().intensity = brightness;
                lense.GetComponent<Light>().intensity = 16.0f;
                flashlight.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = true;
            }
            else if(useTime <= 0.0f)
            {
                flashlight.GetComponent<Light>().intensity = 0.0f;
                lense.GetComponent<Light>().intensity = 0.0f;

                flashlight.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Flashlight>().TurnOffMonsterRender();
                flashlight.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
        else
        {
          //  flashlight.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Flashlight>().TurnOffMonsterRender();
            flashlight.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
            flashlight.GetComponent<Light>().intensity = 0.0f;
            lense.GetComponent<Light>().intensity = 0.0f;

            if (useTime < maxTime )
            {
                useTime += Time.deltaTime;
            }
        }
	}
}
