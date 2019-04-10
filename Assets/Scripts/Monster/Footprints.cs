using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour {

    public MonsterController mon;

    public GameObject leftFootPrint;
    public GameObject rightFootPrint;

    public Transform leftFootLocation;
    public Transform rightFootlocation;

    public float footprintLifeTime = 1.0f;
    public float footOffset = 0.05f;

    void LeftFootStep()
    {
        if(mon.firstTimeDamage)
        {
            RaycastHit hit;

            if (Physics.Raycast(leftFootLocation.position, leftFootLocation.forward, out hit))
            {
                GameObject temp = Instantiate(leftFootPrint, hit.point + hit.normal * footOffset, Quaternion.LookRotation(hit.normal, leftFootLocation.up));
                Destroy(temp, footprintLifeTime);
            }
        }     
    }

    void RightFootStep()
    {
        if (mon.firstTimeDamage)
        {
            RaycastHit hit;

            if (Physics.Raycast(rightFootlocation.position, rightFootlocation.forward, out hit))
            {
                GameObject temp = Instantiate(rightFootPrint, hit.point + hit.normal * footOffset, Quaternion.LookRotation(hit.normal, rightFootlocation.up));
                Destroy(temp, footprintLifeTime);
            }
        }
    }
}
