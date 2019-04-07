using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Rewired;
using UGUIMiniMap;
using Random = UnityEngine.Random;

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
    private Vector3 velocityVector;

    private Animator anim;
    [SyncVar]
    public float x = 0.0f;
    [SyncVar]
    public float y = 0.0f;

    //Head bob, copy pasted from Unity Character Controller
    [Header("Head Bob")]
    [SerializeField] private bool is2D = false;
    [SerializeField] private bl_CurveControlledBob m_HeadBob = new bl_CurveControlledBob();
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private float m_StepInterval;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    private Vector3 m_OriginalCameraPosition;

    //Camera Variables
    public Vector3 smashOffset = new Vector3(0, 2.0f, -5.0f);
    public Vector3 smashViewAngle = new Vector3(90.0f, 0.0f, 0.0f);
    private Vector3 animStartCamPosition;
    private bool duringSmashAnim = false;
    float smashDuration;
    float smashTimer;
    //Percentage of time taken for camera to zoom out/in during smash
    //Example: cameraZoomPercentageTime = 0.25f, camera will zoom out during 1st quarter of the anim, 
    //         stay stationary for 2nd, 3rd quarter, then return to original postition during the 4th
    [Range(0f, 0.5f)] public float cameraZoomPercentageTime = 0.25f;

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
            m_OriginalCameraPosition = fpsCamera.transform.localPosition;
            // fpsCamera.transform.localPosition = CameraPosition.transform.position;

            //Set MiniMap
            FindObjectOfType<bl_MiniMap>().SetTarget(this.gameObject);

            //Set Controls and display to right screen
            player = Rewired.ReInput.players.GetPlayer(controllerNumber);
            GetComponent<MonsterController>().player = player;

            if (!is2D)
            {
                m_HeadBob.Setup(fpsCamera, m_StepInterval);
            }
        }

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        if (hasAuthority)
        {
            UpdateCameraPosition(Time.deltaTime);

            if (duringSmashAnim)
                return;

            InputHandler();
        }
    }

    void InputHandler()
    {
        x = player.GetAxis("VerticalMove");
        y = player.GetAxis("HorizontalMove");

        //Simple Movement
        //Half speed if moving backwards
        float multiplier = (y > 0.0f) ? 1.0f : 0.5f;
        transform.Translate(x * Time.deltaTime * speed * multiplier, 0.0f, y * Time.deltaTime * speed * multiplier);

        velocityVector = new Vector3(x, 0.0f, y);
        velocityVector *= speed;
        velocityVector *= multiplier;

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

    private void UpdateCameraPosition(float deltaTime)
    {
        if (!is2D)
        {
            Vector3 newCameraPosition;
            Vector3 newCameraAngle;
            if (duringSmashAnim)
            {
                smashTimer += deltaTime;
                float normalized = Mathf.Clamp(smashTimer / smashDuration, 0.0f, 1.0f);
                if (normalized < cameraZoomPercentageTime)
                {
                    newCameraPosition = Vector3.Lerp(animStartCamPosition, smashOffset, normalized / cameraZoomPercentageTime);
                    newCameraAngle = Vector3.Lerp(Vector3.zero, smashViewAngle, normalized / cameraZoomPercentageTime);
                }
                else if (normalized > 1.0f - cameraZoomPercentageTime)
                {
                    newCameraPosition = Vector3.Lerp(smashOffset, animStartCamPosition, (normalized - 1.0f + cameraZoomPercentageTime) / cameraZoomPercentageTime);
                    newCameraAngle = Vector3.Lerp(smashViewAngle, Vector3.zero, (normalized - 1.0f + cameraZoomPercentageTime) / cameraZoomPercentageTime);
                }
                else
                {
                    newCameraPosition = fpsCamera.transform.localPosition;
                    newCameraAngle = smashViewAngle;
                }
                if (smashTimer >= smashDuration)
                {
                    newCameraPosition = animStartCamPosition;
                    newCameraAngle = Vector3.zero;
                    duringSmashAnim = false;
                }
                fpsCamera.transform.localPosition = newCameraPosition;
                fpsCamera.transform.localRotation = Quaternion.Euler(newCameraAngle);
                return;
            }
            if (!m_UseHeadBob)
            {
                return;
            }
            if (velocityVector.magnitude > 0.0f)
            {
                fpsCamera.transform.localPosition = m_HeadBob.DoHeadBob(velocityVector.magnitude + (speed * m_RunstepLenghten));
                newCameraPosition = fpsCamera.transform.localPosition;
                newCameraPosition.y = fpsCamera.transform.localPosition.y;
            }
            else
            {
                newCameraPosition = fpsCamera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y;
            }
            fpsCamera.transform.localPosition = newCameraPosition;
        }
        else
        {
            Vector3 p = transform.position;
            fpsCamera.transform.localPosition = Vector3.Lerp(fpsCamera.transform.localPosition, new Vector3(p.x, p.y, m_OriginalCameraPosition.z), Time.deltaTime * 7);
        }
    }

    //Zooming out then in during smash animation, should only be called by anim events at the start of "Smash" anim
    public void StartSmash()
    {
        //Hard coded anim name and speed;
        foreach (AnimationClip ac in anim.runtimeAnimatorController.animationClips)
        {
            if (ac.name == "smash8-test")
            {
                smashDuration = ac.length;
                break;
            }
        }
        smashDuration /= 1.5f;
        animStartCamPosition = fpsCamera.transform.localPosition;
        fpsCamera.transform.localRotation = Quaternion.identity;
        smashTimer = 0.0f;
        duringSmashAnim = true;
    }
}
