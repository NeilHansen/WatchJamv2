using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TerminalFixer : MonoBehaviour {

    public Player player;

    private DoorController doorManager;

    private GameObject hitObject;

    private CapsuleCollider drainCollider;

    public float heldTime;

    // Use this for initialization
    void Start () {
        player = GetComponent<PlayerController>().player;

        // doorManager = GameObject.FindObjectOfType<DoorController>();
        doorManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<DoorController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetButton("Interact"))
        {
            // heldTime = 0.0f;
            heldTime += Time.deltaTime;
            Debug.Log("holding");
            if (hitObject.gameObject.GetComponent<TerminalController>().isBroken == true)
            {
                if (heldTime >= 3.0f)
                {
                    Debug.Log("finished");
                    hitObject.gameObject.GetComponent<TerminalController>().isBroken = false;
                    hitObject.gameObject.GetComponent<TerminalController>().securitySystem.CheckDoors();
                    hitObject.gameObject.GetComponent<MapBlip>().color = Color.white;
                    hitObject.gameObject.GetComponent<MapBlipMonster>().color = Color.white;
                }
            }
            else
            {
                if (this.transform.GetChild(2).GetComponent<FlashlightController>().maxTime < 5)
                {
                    this.transform.GetChild(2).GetComponent<FlashlightController>().maxTime += Time.deltaTime;
                }
            }
            //hitObject.GetComponentInChildren<SecurityButton>().isPressed = false;
            if (doorManager.DoorOpen == true)
            {

                // doorManager.CloseDoors();


            }
            else
            {

                //  doorManager.OpenDoors();

            }
        }
        if (player.GetButtonUp("Interact"))
        {
            heldTime = 0.0f;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terminal" && this.gameObject.name != "Cone")
        {
            hitObject = other.gameObject;
            //   interactUI.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "flash" && this.gameObject.tag == "Monster")
        {
            // Debug.Log("hitting monster");
            // this.GetComponent<MonsterUIController>().SwitchDirection();


        }
        else if (other.gameObject == null && this.gameObject.tag == "Monster")
        {
            //  Debug.Log("not hitting monster");
            //this.GetComponent<MonsterUIController>().isSeen = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            hitObject = other.gameObject;

            //  interactUI.SetActive(false);
        }

        if (other.gameObject.tag == "flash")
        {
            //  Debug.Log("not hitting monster");
            // this.GetComponent<MonsterUIController>().isSeen = false;
        }


    }
}
