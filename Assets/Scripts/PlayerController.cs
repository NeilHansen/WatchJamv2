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

    //Movement and Rotation Varaibles
    public float speed = 5.0f;
    public float rotSpeed = 90.0f;
    public float FOVmin = -30.0f;
    public float FOVmax = 30.0f;

    // Use this for initialization
    void Start()
    {
        //Set Gamemanager with references
        GameManager.Instance.players.Add(this.gameObject);
        GameManager.Instance.playerControllers.Add(this);

        //Give Children References
        tFixer.playerController = this;
        flashLight.playerController = this;

        player = Rewired.ReInput.players.GetPlayer(controllerNumber);

        fpsCamera.targetDisplay = playerNumber;
    }

    void Update()
    {
        InputHandler();
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
