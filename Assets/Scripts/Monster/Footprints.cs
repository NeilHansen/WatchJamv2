using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour {

    public GameObject leftFootPrint;
    public GameObject rightFootPrint;

    public Transform leftFootLocation;
    public Transform rightFootlocation;

    float footprintLifeTime = 1.0f;
    float footOffset = 0.05f;

    void LeftFootStep()
    {
        RaycastHit hit;

        if(Physics.Raycast(leftFootLocation.position, leftFootLocation.forward, out hit))
        {
            GameObject temp = Instantiate(leftFootPrint, hit.point + hit.normal * footOffset, Quaternion.LookRotation(hit.normal, leftFootLocation.up));
            Destroy(temp, footprintLifeTime);
        }
    }

    void RightFootStep()
    {
        RaycastHit hit;

        if (Physics.Raycast(rightFootlocation.position, rightFootlocation.forward, out hit))
        {
            GameObject temp = Instantiate(rightFootPrint, hit.point + hit.normal * footOffset, Quaternion.LookRotation(hit.normal, rightFootlocation.up));
            Destroy(temp, footprintLifeTime);
        }
    }
}
