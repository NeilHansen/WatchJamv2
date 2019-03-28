using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : MonoBehaviour {

    public int sectorNumber;
    public TerminalController[] sectorTerminals;
    public Door sectorExit;
    public int brokenTerminals = 0;
    public bool brokenSector = false;
    private int maxBrokenTerminals;
    private GameManager gm;
	// Use this for initialization
	void Start () {
        gm = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		if(brokenTerminals >= 2)
        {
            brokenSector = true;
        }
	}

    public void CheckTerminals()
    {
        //Reset to get proper count
        brokenTerminals = 0;

        foreach (TerminalController t in sectorTerminals)
        {
            if (t.isBroken)
            {
                brokenTerminals += 1;
            }
        }
    }

    public void OpenDoor()
    {
        sectorExit.MoveUp();
    }
}
