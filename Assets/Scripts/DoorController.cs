using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {


    public Door[] Gates;
    public SecurtySystem[] alarm;
    public bool DoorOpen = false;

    public GameObject path;
    public GameObject seenUI;

    // Use this for initialization
    void Start () {
        Gates = GameObject.FindObjectsOfType<Door>();
        alarm = GameObject.FindObjectsOfType<SecurtySystem>();
	}
	
	// Update is called once per frame
	void Update () {

 

      
		
	}

    public void OpenDoors()
    {
       
        // Debug.Log("OpeningDoors1");
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveUp();
            // Debug.Log("OpeningDoors");
            
        }
    }
    public void CloseDoors()
    {
        
       
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveDown();
           
            
        }
    }

    public void UpdateDoors()
    {
       
        Debug.Log(DoorOpen);
        if (DoorOpen)
        {
            for (int i = 0; i < alarm.Length; i++)
            {
                alarm[i].alarmOff();
               //alarm[i].alarmIncrease = false;
            }

            Debug.Log(DoorOpen);
            OpenDoors();
        }
        else
        {
            for (int i = 0; i < alarm.Length; i++)
            {
                alarm[i].alarmOn();
                //alarm[i].alarmIncrease = true;
            }
            
            CloseDoors();
        }
        // DoorSwitch = false;
      
    }

}
