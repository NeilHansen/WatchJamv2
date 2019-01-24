using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FlashlightController : MonoBehaviour {
    public SecurityController securityController;

    public Light light;
    private MeshRenderer flashMeshRender;
    private MeshCollider flashMeshCollider;
    private MonsterController monsterController;

    private float brightness;
    private float useTime = 0.0f;

	// Use this for initialization
	void Start () {
        flashMeshRender = GetComponent<MeshRenderer>();
        flashMeshCollider = GetComponent<MeshCollider>();

        brightness = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        //Turn on flash light
		if(securityController.player.GetButton("FlashLight"))
        {
            securityController.b_Shinning = true;

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
            }
        }
        //Turn off flash light
        else
        {
            securityController.b_Shinning = false;

            light.intensity = 0.0f;
            flashMeshRender.enabled = false;
            flashMeshCollider.enabled = false;

            //If we used anything then keep refilling it
            if (useTime < securityController.flashLightMaxTime)
            {
                useTime += Time.deltaTime / 2.0f;
            }

            //Were not hitting anything so remove the reference to the controller
            monsterController = null;
        }

        //Update Use time
        SecurityUI.Instance.SetFlashUIValue(useTime);
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

            MonsterUI.Instance.MonsterSeenUI(monsterController);
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
