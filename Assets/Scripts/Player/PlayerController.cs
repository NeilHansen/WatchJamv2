using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    public int controllerNumber;
    public Player player;
    public Camera fpsCamera;

    public TerminalFixer tFixer;
    public FlashlightController flashLight;
    public PlayerStunned stun;

    //Movement and Rotation Varaibles
    public float speed = 5.0f;
    public float rotSpeed = 90.0f;
    public float FOVmin = -30.0f;
    public float FOVmax = 30.0f;

    public bool b_isInteracting = false;
    public bool b_Shinning = false;
    public float TerminalFixTime = 3.0f;
    public float flashLightMaxTime = 5.0f;
    public float stunTime;

    // Use this for initialization
    void Start()
    {
        stun.playerController = this;

        flashLight.UseTime = flashLightMaxTime;

        //Give Children References
        tFixer.playerController = this;
        flashLight.playerController = this;
    }

    void Update()
    {
        InputHandler();
    }

    public void Init(int playerN, int controllerN)
    {
        //Assign Variables
        playerNumber = playerN;
        controllerNumber = controllerN;

        //Set Controls and display to right screen
        player = Rewired.ReInput.players.GetPlayer(controllerNumber);
        fpsCamera.targetDisplay = playerNumber;

        //Set Flashlight variables
        UIManager.Instance.SetFlashUIMaxValue(playerNumber, flashLightMaxTime);
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
