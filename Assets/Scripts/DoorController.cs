using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DoorController : NetworkBehaviour {
    public static DoorController Instance;

    public SectorController[] Sectors;
    private List<SectorController> brokenSectors = new List<SectorController>();

    public bool isOpen = false;
    public int brokenSectorCount = 0;
    public int maxBrokenSectorCount = 6;
    public GameManager gm;
    

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
        gm = GameObject.FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if(isServer)
        {
            if (gm.brokenTerminalCount >= maxBrokenSectorCount && !isOpen)
            {
                OpenDoors();
            }
        }
	}

   

    public void CheckDoors()
    {
        brokenSectorCount = 0;

        foreach (SectorController s in Sectors)
        {
            s.CheckTerminals();
            if (s.brokenTerminals >= 1)
            {
                brokenSectorCount += 1;
            }
        }
    }

    public void OpenDoors()
    {
        isOpen = true;

        foreach (SectorController s in Sectors)
        {
            if (s.brokenTerminals >= 1)
            {
                brokenSectors.Add(s);
            }
        }

        int randomPick = Random.Range(0, 3);
        Debug.Log("Open Door");
        brokenSectors[randomPick].OpenDoor();
    }
}
