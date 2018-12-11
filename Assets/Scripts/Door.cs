using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {


    public float height;

    public float speed;

    private float startTime;
    private float journeyLength;
    private float journeyLengthBack;

    private Vector3 doorStartPos;
    private Vector3 doorEndPos;

    private float fracJourney;
    private float downfracJourney;
    private float distCovered;

    public bool movingUp;
    // Use this for initialization
    void Start () {
        height = this.gameObject.transform.localScale.y;
        startTime = Time.time;
        doorStartPos = this.transform.position;
        doorEndPos = (this.transform.position + new Vector3(0, height, 0));
        journeyLength = Vector3.Distance(doorStartPos, new Vector3(0, height, 0));
        journeyLengthBack = Vector3.Distance(doorEndPos, doorStartPos);

    }
	
	// Update is called once per frame
	void Update () {
         distCovered = (Time.time - startTime) * speed;

         fracJourney = distCovered / journeyLength;
        
         downfracJourney = distCovered / journeyLengthBack;


        if (movingUp)
        {
            journeyLength = Vector3.Distance(doorStartPos, new Vector3(0, height, 0));
            this.transform.position = Vector3.Lerp(doorStartPos, doorEndPos, fracJourney);

        }
        else
        {
            journeyLength = Vector3.Distance(this.transform.position, doorStartPos);
            this.transform.position = Vector3.Lerp(doorEndPos, doorStartPos, downfracJourney);
        }
	}

    public void MoveUp()
    {
        distCovered = 0.0f;
        journeyLength = Vector3.Distance(doorStartPos, doorEndPos);
        movingUp = true;
       
      // this.gameObject.transform.position += new Vector3(0, height,0);

    }

    public void MoveDown()
    {
        distCovered = 0.0f;
        journeyLength = Vector3.Distance(doorEndPos, doorStartPos);
        movingUp = false;
      //  this.gameObject.transform.position -= new Vector3(0, height,0);
      
    }
}
