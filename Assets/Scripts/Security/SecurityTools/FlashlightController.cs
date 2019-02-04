using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FlashlightController : MonoBehaviour
{
    public SecurityController securityController;

    public GameObject origin;
    public GameObject flashlight;
    public MeshRenderer flashMeshRender;
    public float flashlightRange = 10.0f;
    public float raycastRadius = 0.2f;

    // Use this for initialization
    void Start () {
        flashMeshRender.enabled = false;
	}

    //For Normal Light(Always on)
    public void SecurityFlashLightOn()
    {
        //As long as we still have time keep on
        if (!(securityController.flashLightUseTime <= 0.0f))
        {
            // Drawing ray to see
            Debug.DrawRay(origin.transform.position, origin.transform.forward * flashlightRange, Color.yellow);

            RaycastHit hit;
            if (Physics.SphereCast(origin.transform.position, raycastRadius, origin.transform.forward, out hit, flashlightRange))
            {
                if(hit.collider.tag == "Monster")
                {
                    securityController.CmdDamageTarget(hit.collider.gameObject);
                }
            }

            TurnVioletOn();
            securityController.b_UsingFlashLight = true;
        }
        //Turn off
        else
        {
            securityController.CmdTurnLightOff();
            securityController.b_UsingFlashLight = false;
        }
    }

    public void SecurityFlashLightOff()
    {
        TurnVioletOff();
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
}
