using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MonsterController : NetworkBehaviour {
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

    public bool isFlashLightHitting = false;
    public bool isDrainHitting = false;

    public bool isDraining = false;
    public bool drainCooldown = false;
    public bool isPunching = false;
    public bool punchCooldown = false;

    //Monster Transparency
    public float materialAlphaChangeRate = 0.1f;
    [SyncVar(hook = "OnChangeMonsterAlpha")]
    public float currentAlpha = 0.0f;

    private Material monsterMaterial;
    private Color monsterColor;

    // Use this for initialization
    void Start () {
        if (hasAuthority)
        {
            GameManager.Instance.localPlayer = this.gameObject;
            fpsCamera = Camera.main;
            fpsCamera.transform.SetParent(this.transform);
            fpsCamera.transform.localRotation = Quaternion.identity;
            fpsCamera.transform.localPosition = Vector3.zero;

            //Set MiniMap
            FindObjectOfType<bl_MiniMap>().SetTarget(this.gameObject);

            //Set Controls and display to right screen
            player = Rewired.ReInput.players.GetPlayer(controllerNumber);
        }

        //Give children a reference to this script
        powerDrain.monster = this;
        powerPunch.monster = this;

        //Find Material with monster
        monsterMaterial = GameObject.FindGameObjectWithTag("Monster Material").GetComponent<SkinnedMeshRenderer>().material;
        monsterMaterial.color = monsterColor;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hasAuthority)
        {
            if (isFlashLightHitting)
            {
                NetworkTakeDamage();
            }
            if (isDrainHitting)
            {
                NetworkRemoveDamage();
            }

            InputHandler();
            powerDrain.MonsterDrain();
            powerPunch.MonsterPunch();
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

    void NetworkTakeDamage()
    {
        if(isServer)
        {
            RpcTakeDamage();
        }
        else
        {
            CmdTakeDamage();
        }
    }

    void NetworkRemoveDamage()
    {
        if (isServer)
        {
            RpcRemoveDamage();
        }
        else
        {
            CmdRemoveDamage();
        }
    }

    void OnChangeMonsterAlpha(float alpha)
    {
        if (alpha > 0.0)
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetMonsterSeenIcon(true);
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }

            monsterColor.a = currentAlpha;
            monsterMaterial.color = monsterColor;
        }
        else
        {
            if (hasAuthority)
            {
                MonsterUI.Instance.SetMonsterSeenIcon(false);
                MonsterUI.Instance.SetVisibilitySlider(alpha);
            }

            monsterColor.a = 0;
            monsterMaterial.color = monsterColor;
        }
    }

    //This is a Network command, so the damage is done to the relevant GameObject
    [ClientRpc]
    void RpcTakeDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        currentAlpha += damage;
    }

    [ClientRpc]
    void RpcRemoveDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        currentAlpha -= damage;
    }

    //This is a Network command, so the damage is done to the relevant GameObject
    [Command]
    void CmdTakeDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        currentAlpha += damage;
    }

    [Command]
    void CmdRemoveDamage()
    {
        float damage = materialAlphaChangeRate * Time.deltaTime;
        currentAlpha -= damage;
    }
}
