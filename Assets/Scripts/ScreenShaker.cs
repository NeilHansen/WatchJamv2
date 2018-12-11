using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{

    public Camera mainCamera;

    public float shakeDuration;

    public float shakeMagnitude;
   // public float decreaseFactor;

     Vector3 originalPos;

    public void ShakeIt()
    {
        originalPos = mainCamera.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeDuration);
    }

    void StartCameraShaking()
    {
        float cameraSHakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float cameraSHakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;

        Vector3 cameraIntermadiatePosition = mainCamera.transform.position;

        cameraIntermadiatePosition.x += cameraSHakingOffsetX;
        cameraIntermadiatePosition.y += cameraSHakingOffsetY;
        mainCamera.transform.position = cameraIntermadiatePosition;
    }


    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        mainCamera.transform.position = originalPos;
    }








}

