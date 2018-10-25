using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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
    

    private DoorController doorManager;

    // Use this for initialization
    void Start () {
        player = Rewired.ReInput.players.GetPlayer(playerNumber);
        fpsCamera = this.GetComponentInChildren<Camera>();
        doorManager = GameObject.FindObjectOfType<DoorController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        HandleInput();
        
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

        if (player.GetButtonDown("Interact") && Caninteract)
        {
            Debug.Log("Can Interact");
            // Caninteract = false;
            if (this.gameObject.tag == "Monster" && doorManager.DoorOpen == false)
            {
                doorManager.DoorOpen = true;
                doorManager.UpdateDoors();
            }else if (this.gameObject.tag == "Security" && doorManager.DoorOpen == true)
            {
                doorManager.DoorOpen = false;
                doorManager.UpdateDoors();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Terminal")
        {
            Caninteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terminal")
        {
            Caninteract = false;
        }
    }

}
