using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DoorController : NetworkBehaviour {
    public static DoorController Instance;

    public Door[] Gates;
    public bool DoorOpen = false;

    public TerminalController[] Terminals;
    public int brokenTerminalCount;
    public int maxTerminals;

    // Use this for initialization
    void Awake()
    {
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start () {
        Gates = GameObject.FindObjectsOfType<Door>();
        Terminals = GameObject.FindObjectsOfType<TerminalController>();
    }
	
	// Update is called once per frame
	void Update () {
        if(isServer)
        {
            // CheckDoors();
            if (brokenTerminalCount >= maxTerminals)
            {
                Debug.Log("Doors Open");
                OpenDoors();
            }
            else
            {
                Debug.Log("Doors Closed");
                CloseDoors();
            }
        }
	}

     public void CheckDoors()
    {
        brokenTerminalCount =0;
        for(int i =0; i < Terminals.Length  ; i++)
        {
            if (Terminals[i].isBroken)
            {
                 brokenTerminalCount++;   
            }
        }      
    }

    public void OpenDoors()
    {
        DoorOpen = true;
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveUp();          
        }
    }
    public void CloseDoors()
    {
        DoorOpen = false;
        for (int i = 0; i < Gates.Length; i++)
        {
            Gates[i].MoveDown();   
        }
    }
}
