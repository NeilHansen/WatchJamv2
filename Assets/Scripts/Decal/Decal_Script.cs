using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal_Script : MonoBehaviour
{
    public enum colliderPosition {
        Top,
        Bottom,
        Left,
        Right
    }

    public Decal_Manager deManager;
    public GameObject ThisDecal;
    public colliderPosition colliderLocation = colliderPosition.Top;
    
    // Start is called before the first frame update
    void Start()
    {      

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Monster"))
        {
            switch(colliderLocation)
            {
                case colliderPosition.Top:
                    if (!deManager.stained && other.GetComponent<MonsterWallMovement>().onCeilingFloor)
                        StartCoroutine(deManager.TurnOnStain(ThisDecal));
                    break;
                case colliderPosition.Bottom:
                    if (!deManager.stained && other.GetComponent<MonsterWallMovement>().onGroundFloor)
                        StartCoroutine(deManager.TurnOnStain(ThisDecal));
                    break;
                case colliderPosition.Left:
                    if (!deManager.stained)
                        StartCoroutine(deManager.TurnOnStain(ThisDecal));
                    break;
                case colliderPosition.Right:
                    if (!deManager.stained)
                        StartCoroutine(deManager.TurnOnStain(ThisDecal));
                    break;
            }
        }
    }

   
}