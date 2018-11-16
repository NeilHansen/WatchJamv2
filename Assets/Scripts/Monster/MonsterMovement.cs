﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed = 6; // Move speed
    public float turnSpeed = 90; // Turning speed (degrees/second)
    public float lerpSpeed = 10; // Smoothing speed
    public float gravity = 10; // Gravity acceleration(Rounding because decimals suck!)
    public float flipSpeed = 2.0f; // How fast to flip onto wall

    private float distGround; // Distance from character position to ground
    public float deltaGround = 0.2f; // Character is grounded up to this distance
    public float flipRange = 1; // Range to detect target wall

    private bool isGrounded;
    private bool flipping = false; // Flag "I'm flipping to wall";

    private Vector3 surfaceNormal; // Current surface normal
    private Vector3 myNormal; // Character normal

    private Rigidbody rigidbody;
    private Transform myTransform;
    private BoxCollider boxCollider; // drag BoxCollider ref in editor

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        myTransform = transform;

        // Normal starts as character up direction
        myNormal = transform.up;
        // Disable physics rotation
        rigidbody.freezeRotation = true;
        // Distance from transform.position to ground
        distGround = boxCollider.size.y - boxCollider.center.y;
    }

    void FixedUpdate()
    {
        // Apply constant weight force according to character normal:
        rigidbody.AddForce(-gravity * rigidbody.mass * myNormal);
    }

    void Update()
    {
        // If flipping don't update
        if (flipping) return;

        Ray ray;
        RaycastHit hit;
        // Drawing ray to see
        Debug.DrawRay(transform.position, transform.forward * flipRange, Color.yellow);

        ray = new Ray(myTransform.position, myTransform.forward);
        if (Physics.Raycast(ray, out hit, flipRange))
        {
            // Wall ahead?
            // Yes: jump to the wall
            FlipToWall(hit.point, hit.normal);
        }
        else if (isGrounded)
        { 
            // No: if grounded, don't do anything  
        }

        // Update surface normal and isGrounded:
        // Cast ray downwards
        ray = new Ray(myTransform.position, -myNormal);
        if (Physics.Raycast(ray, out hit))
        { 
            // Use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            // Assume usual ground normal to avoid "falling forever"
            surfaceNormal = Vector3.up;
        }

        // Lerping
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // Find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
        // Align character to the new myNormal while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed * Time.deltaTime);

        //Movement code - turn left/right with Horizontal axis:
        myTransform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        //Move the character forth/back with Vertical axis:
        myTransform.Translate(0, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
    }

    private void FlipToWall(Vector3 point, Vector3 normal)
    {
        // Signal it's flppnig to wall
        flipping = true;
        // Disable physics while jumping
        rigidbody.isKinematic = true;
        Vector3 orgPos = myTransform.position;
        Quaternion orgRot = myTransform.rotation;
        // Will flip to 0.5 above wall, so were not inside the wall
        Vector3 dstPos = point + normal * (distGround + 0.5f);
        Vector3 myForward = Vector3.Cross(myTransform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(flipTime(orgPos, orgRot, dstPos, dstRot, normal));
    }

    private IEnumerator flipTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime * flipSpeed;
            myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
            myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; 
        }
        // Update myNormal
        myNormal = normal;
        // Enable physics
        rigidbody.isKinematic = false;
        // Jumping to wall finished
        flipping = false; 

    }

}
