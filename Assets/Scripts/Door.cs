using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {


    public float height;
    // Use this for initialization
    void Start () {
        height = this.gameObject.transform.localScale.y;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveUp()
    {

        this.gameObject.transform.position += new Vector3(0, height,0);
    }

    public void MoveDown()
    {
        this.gameObject.transform.position -= new Vector3(0, height,0);
    }
}
