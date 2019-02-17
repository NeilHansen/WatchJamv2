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

    public Light lenseLight;
    private Color lenseLightDefault;
    public Color UVLightColor;

    private MonsterController monster;

    // Use this for initialization
    void Start () {
        lenseLightDefault = lenseLight.color;
        flashMeshRender.enabled = false;
	}

    //For Normal Light(Always on)
    public void SecurityFlashLight()
    {
        RaycastHit hit;
        if (Physics.SphereCast(origin.transform.position, raycastRadius, origin.transform.forward, out hit, flashlightRange))
        {
            if (hit.collider.tag == "Monster")
            {
                monster = hit.collider.gameObject.GetComponent<MonsterController>();
                securityController.CmdShowMonster(monster.gameObject);
            }
            else if(monster != null)
            {
                securityController.CmdHideMonster(monster.gameObject);
                //Reset so we don't keep calling this
                monster = null;
            }
        }
    }

    //For UV Light
    public void SecurityUVFlashLightOn()
    {
        //As long as we still have time keep on
        if (!(securityController.flashLightUseTime <= 0.0f))
        {
            // Drawing ray to see
            Debug.DrawRay(origin.transform.position, origin.transform.forward * flashlightRange, Color.cyan);

            RaycastHit hit;
            if (Physics.SphereCast(origin.transform.position, raycastRadius, origin.transform.forward, out hit, flashlightRange))
            {
                if(hit.collider.tag == "Monster")
                {
                    securityController.CmdDamageTarget(hit.collider.gameObject);
                }
            }

            TurnUVOn();
            securityController.b_UsingFlashLight = true;
        }
        //Turn off
        else
        {
            securityController.CmdTurnUVLightOff();
            securityController.b_UsingFlashLight = false;
        }
    }

    public void SecurityUVFlashLightOff()
    {
        TurnUVOff();
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
    public void TurnUVOn()
    {
        lenseLight.color = UVLightColor;
        flashMeshRender.enabled = true;
    }

    public void TurnUVOff()
    {
        lenseLight.color = lenseLightDefault;
        flashMeshRender.enabled = false;
    }
}
