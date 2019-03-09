using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FlashlightController : MonoBehaviour
{
    public SecurityController securityController;

    public GameObject CenterPoint;
    public GameObject flashlight;
    public float flashlightRange = 10.0f;
    public float fieldOfViewAngle = 110f;

    public Light lenseLight;
    private Color lenseLightDefault;
    public Color UVLightColor;

    public MonsterController monster;
    private bool canHurt = false;

    public GameObject monsterSpotted;
    public float UpdateTime = 1.0f;
    public bool isSpotted = false;

    // Use this for initialization
    void Start () {
        lenseLightDefault = lenseLight.color;
	}

    void OnTriggerStay(Collider other)
    {
        if (monster == null)
        {
            if (other.gameObject.tag == "Monster")
                monster = other.GetComponent<MonsterController>();
        }
        else
        {
            if (other.gameObject.tag == "Monster")
            {
                if (!isSpotted)
                {
                    isSpotted = true;
                    StartCoroutine(InstatiateSpottedIcon(other));
                }

                canHurt = true;

                Vector3 direction = monster.transform.position - CenterPoint.gameObject.transform.position;
                float angle = Vector3.Angle(direction, CenterPoint.gameObject.transform.forward);

                if (angle < fieldOfViewAngle * 0.5)
                {
                    // Drawing ray to see
                    Debug.DrawRay(CenterPoint.gameObject.transform.position, direction.normalized * 1000, Color.yellow);

                    RaycastHit hit;

                    if (Physics.Raycast(CenterPoint.gameObject.transform.position, direction.normalized, out hit))
                    {
                        Debug.Log(hit.collider.gameObject.name);

                        if (hit.collider.gameObject.tag == "Monster")
                        {
                            //Do Once check if true
                            if(!CheckSecurityIsSeen(monster, securityController.networkPlayer.playerNumber))
                            {
                                SwitchSecurityIsSeen(monster, securityController.networkPlayer.playerNumber, true);
                            }
                        }
                        else
                        {
                            if (CheckSecurityIsSeen(monster, securityController.networkPlayer.playerNumber))
                            {
                                SwitchSecurityIsSeen(monster, securityController.networkPlayer.playerNumber, false);
                            }
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            canHurt = false;

            SwitchSecurityIsSeen(monster, securityController.networkPlayer.playerNumber, false);
        }
    }

    private IEnumerator InstatiateSpottedIcon(Collider monster)
    {
        GameObject temp = Instantiate(monsterSpotted, monster.transform.position, monster.transform.rotation);
        yield return new WaitForSeconds(UpdateTime);
        isSpotted = false;
        Destroy(temp);
    }

    private bool CheckSecurityIsSeen(MonsterController m, int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return m.Security1InSight;
            case 2:
                return m.Security2InSight;
            case 3:
                return m.Security3InSight;

            default:
                return false;
        }
    }

    private void SwitchSecurityIsSeen(MonsterController m, int playerNumber, bool b)
    {
        switch(playerNumber)
        {
            case 1:
                Debug.Log(playerNumber + ": " + b);
                m.Security1InSight = b;
                break;
            case 2:
                Debug.Log(playerNumber + ": " + b);
                m.Security2InSight = b;
                break;
            case 3:
                Debug.Log(playerNumber + ": " + b);
                m.Security3InSight = b;
                break;
        }
    }

    //For UV Light
    public void SecurityUVFlashLightOn()
    {
        //As long as we still have time keep on
        if (!(securityController.flashLightUseTime <= 0.0f))
        {
            if(monster != null)
            {
                Vector3 direction = monster.transform.position - CenterPoint.gameObject.transform.position;
                float angle = Vector3.Angle(direction, CenterPoint.gameObject.transform.forward);

                if (angle < fieldOfViewAngle * 0.5)
                {
                    // Drawing ray to see
                    Debug.DrawRay(CenterPoint.gameObject.transform.position, direction.normalized * 1000, Color.red);

                    RaycastHit hit;

                    if (Physics.Raycast(CenterPoint.gameObject.transform.position, direction.normalized, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Monster")
                        {
                            if (!monster.CheckSecurityDamage(securityController.networkPlayer.playerNumber) && canHurt)
                                securityController.CmdDamageTarget(monster.gameObject, securityController.networkPlayer.playerNumber, true);
                        }
                        else
                        {
                            if (monster.CheckSecurityDamage(securityController.networkPlayer.playerNumber))
                                securityController.CmdDamageTarget(monster.gameObject, securityController.networkPlayer.playerNumber, false);
                        }
                    }
                }
            }

            TurnUVOn();
            securityController.b_UsingFlashLight = true;
        }
        //Turn off
        else
        {
            if (monster != null)
            {
                if (monster.CheckSecurityDamage(securityController.networkPlayer.playerNumber))
                    securityController.CmdDamageTarget(monster.gameObject, securityController.networkPlayer.playerNumber, false);
            }

            securityController.CmdTurnUVLightOff();
            securityController.b_UsingFlashLight = false;
        }
    }

    public void SecurityUVFlashLightOff()
    {
        if (monster != null)
        {
            if (monster.CheckSecurityDamage(securityController.networkPlayer.playerNumber))
                securityController.CmdDamageTarget(monster.gameObject, securityController.networkPlayer.playerNumber, false);
        }

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
    }

    public void TurnUVOff()
    {
        lenseLight.color = lenseLightDefault;
    }
}
