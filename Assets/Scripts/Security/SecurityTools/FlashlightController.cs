using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FlashlightController : MonoBehaviour
{
    public SecurityController securityController;

    public MeshRenderer flashMeshRender;
    public MeshCollider flashMeshCollider;

    public float useTime = 0.0f;

	// Use this for initialization
	void Start () {
        flashMeshRender.enabled = false;
        flashMeshCollider.enabled = false;
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

            TurnVioletOn();
            flashMeshCollider.enabled = true;
        }
        //Turn off
        else
        {
            securityController.CmdTurnLightOff();
            flashMeshCollider.enabled = false;
        }
    }

    public void TurnVioletOn()
    {
        flashMeshRender.enabled = true;
    }

    public void SecurityFlashLightOff()
    {
        TurnVioletOff();
        flashMeshCollider.enabled = false;

        //If we used anything then keep refilling it
        if (useTime < securityController.flashLightMaxTime)
        {
            useTime += Time.deltaTime / 2.0f;
        }
    }

    public void TurnVioletOff()
    {
        flashMeshRender.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            securityController.CmdDamageTarget(other.gameObject);
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
