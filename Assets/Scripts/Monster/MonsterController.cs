using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour {

    public int playerNumber;
    public int controllerNumber;
    public Player player;
    public Camera fpsCamera;

    //Movement and Rotation Varaibles
    public float speed = 5.0f;
    public float rotSpeed = 90.0f;
    public float FOVmin = -30.0f;
    public float FOVmax = 30.0f;

    public PowerDrain powerDrain;
    public PowerPunch powerPunch;

    //Drain and punch Variables
    public float stunTime = 3.0f;
    public float punchLength = 3.0f;
    public float punchCooldownLength = 3.0f;

    public float drainLength = 3.0f;
    public float drainCooldownLength = 3.0f;

    public bool isHittingPlayer = false;

    public bool isDraining = false;
    public bool drainCooldown = false;
    public bool isPunching = false;
    public bool punchCooldown = false;

    //Monster Transparency
    public float materialAlphaChangeRate = 0.1f;

    public Material monsterMaterial;
    public Color monsterColor;

    // Use this for initialization
    void Start () {
        //Give children a reference to this script
        powerDrain.monster = this;
        powerPunch.monster = this;

        //Find Material with monster
        monsterMaterial = GameObject.FindGameObjectWithTag("Monster Material").GetComponent<SkinnedMeshRenderer>().material;
        monsterMaterial.color = monsterColor;
    }
	
	// Update is called once per frame
	void Update () {
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
        if(player.GetAxis("RotVertical") > 0)
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
