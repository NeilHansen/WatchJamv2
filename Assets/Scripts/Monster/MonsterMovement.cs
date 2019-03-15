using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Rewired;

public class MonsterMovement : NetworkBehaviour {

   // public GameObject CameraPosition;
    public int controllerNumber = 0;
    public Player player;
    public Camera fpsCamera;

    //Movement and Rotation Varaibles
    public float speed = 5.0f;
    public float rotSpeed = 90.0f;
    public float FOVmin = -30.0f;
    public float FOVmax = 30.0f;

    // Use this for initialization
    void Start () {
        if (hasAuthority)
        {
            GameManager.Instance.localPlayer = this.gameObject;
            GameManager.Instance.isMonster = true;
            fpsCamera = Camera.main;
            fpsCamera.transform.SetParent(this.transform);
            fpsCamera.transform.localRotation = Quaternion.identity;
            fpsCamera.transform.localPosition = new Vector3(0, 1.5f, 0); //Vector3.zero;
         // fpsCamera.transform.localPosition = CameraPosition.transform.position;

            //Set MiniMap
            FindObjectOfType<bl_MiniMap>().SetTarget(this.gameObject);

            //Set Controls and display to right screen
            player = Rewired.ReInput.players.GetPlayer(controllerNumber);
            GetComponent<MonsterController>().player = player;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(hasAuthority)
        {
            InputHandler();
        }
    }

    void InputHandler()
    {
        //Simple Movement
        transform.Translate(player.GetAxis("VerticalMove") * Time.deltaTime * speed, 0.0f, player.GetAxis("HorizontalMove") * Time.deltaTime * speed);

        //Converting Angles to negation
        float currentRotationX = fpsCamera.transform.localEulerAngles.x;
        currentRotationX = (currentRotationX > 180) ? currentRotationX - 360 : currentRotationX;

        //Left and right rotation
        transform.Rotate(0.0f, player.GetAxis("RotHorizontal") * rotSpeed * Time.deltaTime, 0.0f);

        //Looking Up
        if (player.GetAxis("RotVertical") > 0)
        {
            //Check if greater then our FOVmin
            if (!(currentRotationX <= FOVmin))
            {
                fpsCamera.transform.Rotate(player.GetAxis("RotVertical") * rotSpeed * Time.deltaTime * -1.0f, 0.0f, 0.0f);
            }
        }

        //Looking Down
        if (player.GetAxis("RotVertical") < 0)
        {
            //Check if were greater then our FOVmax
            if (!(currentRotationX >= FOVmax))
            {
                fpsCamera.transform.Rotate(player.GetAxis("RotVertical") * rotSpeed * Time.deltaTime * -1.0f, 0.0f, 0.0f);
            }
        }
    }
}
