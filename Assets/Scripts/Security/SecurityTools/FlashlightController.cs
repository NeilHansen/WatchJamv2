using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FlashlightController : MonoBehaviour
{
    public SecurityController securityController;

    public Light light;
    public MeshRenderer flashMeshRender;
    public MeshCollider flashMeshCollider;
    private MonsterController monsterController;

    private float brightness;
    public float useTime = 0.0f;

	// Use this for initialization
	void Start () {
        brightness = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
             
    }

    public void SecurityFlashLightOn()
    {
        //As long as we still have time keep on
        if (!(useTime <= 0.0f))
        {
            useTime -= Time.deltaTime;

            light.intensity = brightness;
            flashMeshRender.enabled = true;
            flashMeshCollider.enabled = true;
        }
        //Turn off
        else
        {
            light.intensity = 0.0f;
            flashMeshRender.enabled = false;
            flashMeshCollider.enabled = false;
            //Stop hitting monster
            monsterController.isFlashLightHitting = false;
        }
    }

    public void SecurityFlashLightOff()
    {
        light.intensity = 0.0f;
        flashMeshRender.enabled = false;
        flashMeshCollider.enabled = false;

        //If we used anything then keep refilling it
        if (useTime < securityController.flashLightMaxTime)
        {
            useTime += Time.deltaTime / 2.0f;
        }

        if (monsterController)
        {
            //Stop hitting monster
            monsterController.isFlashLightHitting = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            //Get the other component once so were not spamming it
            if(monsterController == null)
            {
                monsterController = other.gameObject.GetComponent<MonsterController>();
            }

            monsterController.isFlashLightHitting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            //Get the other component once so were not spamming it
            if (monsterController == null)
            {
                monsterController = other.gameObject.GetComponent<MonsterController>();
            }

            monsterController.isFlashLightHitting = false;
        }
    }

    public float UseTime
    {
        set
        {
            useTime = value;
        }
    }
}
