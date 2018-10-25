using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {


    public Door[] Gates;
    public bool DoorOpen = false;

    // Use this for initialization
    void Start () {
        Gates = GameObject.FindObjectsOfType<Door>();
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
        Debug.Log("ClosingDoors1");
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveDown();
            Debug.Log("ClosingDoors");
        }
    }

    public void UpdateDoors()
    {
        Debug.Log("Here");
        Debug.Log(DoorOpen);
        if (DoorOpen)
        {
           
            Debug.Log("Open");
            Debug.Log(DoorOpen);
            OpenDoors();
        }
        else
        {
          
            Debug.Log("Closed");
            CloseDoors();
        }
        // DoorSwitch = false;
        Debug.Log(DoorOpen + "fuck");
    }

}
