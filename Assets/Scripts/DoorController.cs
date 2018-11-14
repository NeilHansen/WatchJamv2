using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {


    public Door[] Gates;
    public SecurtySystem[] alarm;
    public bool DoorOpen = false;

    public GameObject path;
    public GameObject seenUI;

    public AudioSource alarmSound;

    public TerminalController[] Terminals;
    public int brokenTerminalCount;
    public int maxTerminals;
    // Use this for initialization
    void Start () {
        Gates = GameObject.FindObjectsOfType<Door>();
         alarm = GameObject.FindObjectsOfType<SecurtySystem>();
        // alarm = GameObject.FindGameObjectsWithTag("SecuritySystem")GetComponent<SecurtySystem>();
        Terminals = GameObject.FindObjectsOfType<TerminalController>();
    }
	
	// Update is called once per frame
	void Update () {
       // CheckDoors();
        if(brokenTerminalCount >= maxTerminals)
        {
            Debug.Log("Doors Open");
        }
        else
        {
            Debug.Log("Doors Closed");
        }

      
		
	}

     public void CheckDoors()
    {
        brokenTerminalCount =0;
        Debug.Log(Terminals.Length);
        for(int i =0; i < Terminals.Length  ; i++)
        {
            //int tempBrokenTerminalCount = 0;
            if (Terminals[i].isBroken)
            {

                 brokenTerminalCount ++;
                
               // 
               // brokenTerminalCount = tempBrokenTerminalCount;
            }
            else
            {
               // brokenTerminalCount--;
            }
        }

        foreach(TerminalController terminal in Terminals)
        {
            int tempBrokenTerminalCount = brokenTerminalCount;
            if (terminal.isBroken)
            {
                //int tempBrokenTerminalCount = brokenTerminalCount -1;
               // tempBrokenTerminalCount +=1;
              //  brokenTerminalCount = tempBrokenTerminalCount;
            }
        }
        
    }

    public void OpenDoors()
    {
        DoorOpen = true;
        // Debug.Log("OpeningDoors1");
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveUp();
            // Debug.Log("OpeningDoors");
            
        }

        for (int i = 0; i < alarm.Length; i++)
        {
            alarm[i].alarmOff();
            //alarm[i].alarmIncrease = false;
        }
        alarmSound.Stop();
    }
    public void CloseDoors()
    {
        DoorOpen = false;
       
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveDown();
           
            
        }

        for (int i = 0; i < alarm.Length; i++)
        {
            alarm[i].alarmOn();
            //alarm[i].alarmIncrease = true;
        }

        alarmSound.Play();
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
