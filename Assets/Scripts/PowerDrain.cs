using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDrain : MonoBehaviour {


    public bool isDraining;
    private GameObject hitObject = null;
    // Use this for initialization
    void Start () {
		
	}

    private void OnTriggerStay(Collider other)
    {

        if(other.gameObject.tag == "Security")
        {
            hitObject = other.gameObject;
            isDraining = true;
           // other.gameObject.transform.GetChild(2).GetComponent<FlashlightController>().useTime -= 2 * Time.deltaTime;
           // other.gameObject.transform.GetChild(2).GetComponent<FlashlightController>().maxTime -= 2 * Time.deltaTime;
          //  other.gameObject.transform.parent.GetComponent<MonsterUIController>().isDraining = true;
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Security")
        {
            hitObject = other.gameObject;
            isDraining = false;
            // other.gameObject.transform.GetChild(2).GetComponent<FlashlightController>().useTime -= 2 * Time.deltaTime;
            // other.gameObject.transform.GetChild(2).GetComponent<FlashlightController>().maxTime -= 2 * Time.deltaTime;
            //  other.gameObject.transform.parent.GetComponent<MonsterUIController>().isDraining = true;
        }

    }



    // Update is called once per frame
    void Update () {
        if (isDraining)
        {
            
            if (hitObject != null && hitObject.transform.GetChild(2).GetComponent<FlashlightController>().useTime > 0.0f)
            {
                hitObject.transform.GetChild(2).GetComponent<FlashlightController>().useTime -= 2 * Time.deltaTime;
               // hitObject.transform.GetChild(2).GetComponent<FlashlightController>().maxTime -= 2 * Time.deltaTime;
                this.GetComponentInParent<MonsterUIController>().isDraining = true;
                Debug.Log(hitObject.gameObject.name + " Draining Power");
            }
        }
        else
        {
            if (hitObject != null)
            {
               // hitObject.transform.GetChild(2).GetComponent<FlashlightController>().useTime -= 2 * Time.deltaTime;
               // hitObject.transform.GetChild(2).GetComponent<FlashlightController>().maxTime -= 2 * Time.deltaTime;
                this.GetComponentInParent<MonsterUIController>().isDraining = false;
                Debug.Log(hitObject.gameObject.name + " Not Draining Power");
            }
            
        }
	}
}
