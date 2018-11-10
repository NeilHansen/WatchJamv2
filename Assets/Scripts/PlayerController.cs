using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    private Player player;
    public int playerNumber;
    private Camera fpsCamera;

    public float speed;
    public float rotSpeed;


    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public float verticalCameraSpeed;
    public float horizontalCameraSpeed;
    
    public float FOVmin;
    public float FOVmax;

    private bool Caninteract = false;

    private float punchLength;

    private DoorController doorManager;

    public GameObject interactUI;


    public NavMeshAgent meshAgent;

    public Transform trail;


    private bool ShowTrail;

    private LineRenderer line;

    // Use this for initialization
    void Start () {
        player = Rewired.ReInput.players.GetPlayer(playerNumber);
        fpsCamera = this.GetComponentInChildren<Camera>();
       // doorManager = GameObject.FindObjectOfType<DoorController>();
        doorManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<DoorController>();
        line = GetComponent<LineRenderer>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        
    }

     void Update()
    {

        HandleInput();
        if (player.GetButtonDown("Interact") && Caninteract && this.gameObject.tag == "Monster")
        {
            if (doorManager.DoorOpen == false)
            {
                doorManager.DoorOpen = true;
                doorManager.UpdateDoors();
            }
        }

        if (player.GetButtonDown("Interact") && Caninteract && this.gameObject.tag == "Security")
        {
            if (doorManager.DoorOpen == true)
            {
                // doorManager.DoorOpen = true;
                //  doorManager.UpdateDoors();
                doorManager.CloseDoors();
                Debug.Log("HERE");
            }
            else
            {
                Debug.Log("HERE2");
                doorManager.OpenDoors();
                //  doorManager.DoorOpen = false;
                //  doorManager.UpdateDoors();
            }
        }

        if(ShowTrail)
        {

        }
    }


    
    void HandleInput()
    {
        if(player.GetAxis("HorizontalMove") > 0.0f)
        {
          //  Debug.Log("MoveForward");
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (player.GetAxis("HorizontalMove") < 0.0f)
        {
        //    Debug.Log("MoveBackward");
            transform.position -= transform.forward * Time.deltaTime * speed;
        }
        if (player.GetAxis("VerticalMove") > 0.0f)
        {
           // Debug.Log("MoveRight");
            transform.position += transform.right * Time.deltaTime * speed;
        }
        if (player.GetAxis("VerticalMove") < 0.0f)
        {
           // Debug.Log("MoveLeft");
            transform.position -= transform.right * Time.deltaTime * speed;
        }

        //right and left movement uncomment if needed
        /* if (player.GetAxis("VerticalMove") > 0.0f)
         {
             Debug.Log("MoveRight");
             transform.position += transform.right * Time.deltaTime;
         }
         if (player.GetAxis("VerticalMove") < 0.0f)
         {
             Debug.Log("MoveLeft");
             transform.position -= transform.right * Time.deltaTime;
         }
         */


        //right stick camera rotation
        float rotX = player.GetAxis("RotHorizontal") * rotSpeed;
        float rotY = player.GetAxis("RotVertical") * rotSpeed;

        //camera rotation
        if (rotX != 0.0f || rotY != 0.0f)
        {
           // Debug.Log("Rotate");
            yaw += horizontalCameraSpeed * player.GetAxis("RotHorizontal");
             pitch -= verticalCameraSpeed * player.GetAxis("RotVertical");
          
             pitch = Mathf.Clamp(pitch, FOVmin, FOVmax);
             transform.localEulerAngles = new Vector3(0.0f, yaw, 0.0f);
            fpsCamera.transform.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
        }


        if (player.GetButtonDown("Interact") && Caninteract && this.gameObject.tag == "Monster")
        {
            if (doorManager.DoorOpen == false)
            {
             //   doorManager.DoorOpen = true;
          //      doorManager.UpdateDoors();
            }
        }

        if (player.GetButtonDown("Interact") && Caninteract && this.gameObject.tag == "Security")
        {
            if (doorManager.DoorOpen == true)
            {
                // doorManager.DoorOpen = true;
                //  doorManager.UpdateDoors();
             //   doorManager.CloseDoors();
                Debug.Log("HERE");
            }
            else
            {
                Debug.Log("HERE2");
             //   doorManager.OpenDoors();
              //  doorManager.DoorOpen = false;
              //  doorManager.UpdateDoors();
            }
        }


            if (player.GetButtonDown("Interact") && Caninteract)
        {
            Debug.Log("Can Interact");
            // Caninteract = false;
            if (this.gameObject.tag == "Monster" )
            {
                if(doorManager.DoorOpen == false)
                {
                  //  doorManager.DoorOpen = true;
                  //  doorManager.UpdateDoors();
                }
               

            }
            else if(this.gameObject.tag == "Security")
            {
                if (doorManager.DoorOpen == false)
                {
                  //  doorManager.DoorOpen = true;
                   // doorManager.UpdateDoors();
                }
                else 
                {
                  //  doorManager.DoorOpen = false;
                   // doorManager.UpdateDoors();
                }
            }

                if (doorManager.DoorOpen == false)
                {
                //    doorManager.DoorOpen = true;
                 //   doorManager.UpdateDoors();
                }
                else
                {
                //    doorManager.DoorOpen = false;
                //    doorManager.UpdateDoors();
                }
            

            if (this.gameObject.tag == "Security" && doorManager.DoorOpen == true)
            {
               // doorManager.DoorOpen = false;
               // doorManager.UpdateDoors();
            }
          //  else if(this.gameObject.tag == "Security" && doorManager.DoorOpen == false)
          //  {
           //     doorManager.DoorOpen = true;
           //     doorManager.UpdateDoors();
           // }


        }


        //monster punch
        punchLength -= Time.deltaTime;
        if (punchLength < 0.0f)
        {
            Debug.Log("Can Punch");
            if (player.GetButtonDown("Punch") && this.gameObject.tag == "Monster")
            {
                this.transform.GetChild(1).gameObject.GetComponent<Animation>().Play("attack2");
                this.transform.GetChild(0).gameObject.SetActive(true);
                fpsCamera.gameObject.GetComponent<CameraController>().cameraOffset = new Vector3(0, 0, 0);
                fpsCamera.gameObject.GetComponent<CameraController>().cameraDistance = 2;
                punchLength = 2.5f;
              //  Debug.Log("Punched");
            }
        }
        else if (punchLength < 1.0f)
        {
            this.transform.GetChild(1).gameObject.GetComponent<Animation>().Play("idleLookAround");
            this.transform.GetChild(0).gameObject.SetActive(false);
            fpsCamera.gameObject.GetComponent<CameraController>().cameraOffset = new Vector3(0, 2.5f, 0);
            fpsCamera.gameObject.GetComponent<CameraController>().cameraDistance = 0;
           // Debug.Log("Cant Punch");
        }

        //security flashlight
        if(player.GetButtonDown("FlashLight") && this.gameObject.tag == "Security")
        {
       
            this.transform.GetChild(2).gameObject.GetComponent<FlashlightController>().shine = true;
        }
        else if (player.GetButtonUp("FlashLight") && this.gameObject.tag == "Security")
        {
            this.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
            this.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Flashlight>().TurnOffMonsterRender();
            this.transform.GetChild(2).gameObject.GetComponent<FlashlightController>().shine = false;
          
        }

        //monster directions
        if(player.GetButton("ShowDirection")&& this.gameObject.tag == "Monster")
        {
           // doorManager.path.SetActive(true);
            GameObject.FindObjectOfType<Flashlight>().TurnOnMonsterRender();
            doorManager.seenUI.SetActive(true);

           Exit exit =  FindObjectOfType<Exit>();

            if(exit != null && line != null)
            {
                meshAgent.SetDestination(exit.transform.position);
                Vector3 start = this.transform.position;
                
                line.positionCount = meshAgent.path.corners.Length;
               // line = Instantiate(line, transform);
                for(int i = 0; i < meshAgent.path.corners.Length; i++ )
                {
                    Debug.DrawLine(start, meshAgent.path.corners[i], Color.green);
                    start = meshAgent.path.corners[i];
                    line.SetPosition(i, meshAgent.path.corners[i]);
                }
                foreach (Vector3 v in meshAgent.path.corners)
                {
                   // Debug.DrawLine(start, v);
                    start = v;
                  //  line = Instantiate(line, transform);
                 //   line.SetPosition(v);
                  //  line.SetPosition(1, v);
                }
              //  Instantiate(trail, this.transform.position + new Vector3(0,0,1), transform.rotation);
                
            }
        }

        if(player.GetButtonUp("ShowDirection") && this.gameObject.tag == "Monster")
        {
          //  doorManager.path.SetActive(false);
            GameObject.FindObjectOfType<Flashlight>().TurnOffMonsterRender();
            doorManager.seenUI.SetActive(false);
            //this.transform.GetChild(2).gameObject.GetComponent<FlashlightController>().flashlight.gameObject.GetComponent<Flashlight>().TurnOffMonsterRender();


        }

        //Interact UI

        if(Caninteract)
        {
            interactUI.SetActive(true);
        }
        else
        {
            interactUI.SetActive(false);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Terminal" && this.gameObject.name != "Cone")
        {
            Caninteract = true;
         //   interactUI.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "flash" && this.gameObject.tag == "Monster")
        {
           // Debug.Log("hitting monster");
           // this.GetComponent<MonsterUIController>().SwitchDirection();


        }
        else if(other.gameObject == null &&this.gameObject.tag == "Monster")
        {
          //  Debug.Log("not hitting monster");
            //this.GetComponent<MonsterUIController>().isSeen = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            Caninteract = false;
          //  interactUI.SetActive(false);
        }

        if(other.gameObject.tag == "flash")
        {
          //  Debug.Log("not hitting monster");
           // this.GetComponent<MonsterUIController>().isSeen = false;
        }

      
    }

}
