using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraController : MonoBehaviour
{

    public int playerId = 0; // The Rewired player id of this character

    private float currentX = 0.0f;
    private float currentY = 45.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;

    public Transform player;
    private Transform cameraTransform;

    public float cameraAngleMinY = -35.0f;
    public float cameraAngleMaxY = 50.0f;
    public float cameraDistance = 10.0f;
    public Vector3 cameraOffset = new Vector3(0, 0, 0);
    public float smoothSpeed = 0.125f;

    [SerializeField] private float m_cameraRotationSpeed = 2f;

    private Player playerControl; // The Rewired Player

    private void Awake()
    {
        // Get the Rewired object for this player and keep it for the duration of the character's lifetime
        playerControl = ReInput.players.GetPlayer(playerId);
    }

    private void Start()
    {
       this.transform.position = cameraOffset;
        cameraTransform = transform;
       // 
    }

    private void Update()
    {
       // HandleInput();
        //clamp camera y rotation
        currentY = Mathf.Clamp(currentY, cameraAngleMinY, cameraAngleMaxY);
    }

    private void HandleInput()
    {
       // currentX += playerControl.GetAxis("RotHorizontal") * m_cameraRotationSpeed;
      //  currentY += playerControl.GetAxis("RotVertical") * m_cameraRotationSpeed;
    }

    private void LateUpdate()
    {
        //get direction
        Vector3 direction = new Vector3(0, 0, -cameraDistance);
        //rotation
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        //move camera to player at the correct rotation and direction 
        cameraTransform.position = player.position + rotation * direction;
        //to smooth out camera moving to the player
        Vector3 desiredPosition = player.position + cameraOffset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        cameraTransform.position = smoothPosition;
       // cameraTransform.LookAt(player.position);
    }
}