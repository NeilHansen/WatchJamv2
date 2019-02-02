using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FlashlightController : MonoBehaviour
{
    public SecurityController securityController;

    public GameObject flashlight;
    public MeshRenderer flashMeshRender;
    public MeshCollider flashMeshCollider;

	// Use this for initialization
	void Start () {
        flashMeshRender.enabled = false;
        flashMeshCollider.enabled = false;
	}

    //For Normal Light(Always on)
    public void SecurityFlashLightOn()
    {
        //As long as we still have time keep on
        if (!(securityController.flashLightUseTime <= 0.0f))
        {
            TurnVioletOn();
            flashMeshCollider.enabled = true;
            securityController.b_UsingFlashLight = true;
        }
        //Turn off
        else
        {
            securityController.CmdTurnLightOff();
            flashMeshCollider.enabled = false;
            securityController.b_UsingFlashLight = false;
        }
    }

    public void SecurityFlashLightOff()
    {
        TurnVioletOff();
        flashMeshCollider.enabled = false;
        securityController.b_UsingFlashLight = false;
    }

    public void ToggleFlashLightOn()
    {
        flashlight.SetActive(true);
    }

    public void ToggleFlashLightOff()
    {
        flashlight.SetActive(false);
    }

    //For Violet Light
    public void TurnVioletOn()
    {
        flashMeshRender.enabled = true;
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
}
