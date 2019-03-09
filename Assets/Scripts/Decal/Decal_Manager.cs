using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal_Manager : MonoBehaviour {

    public bool stained = false;
    public float LifeTime = 5.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator TurnOnStain(GameObject g)
    {
        stained = true;
        g.SetActive(true);
        yield return new WaitForSeconds(LifeTime);
        stained = false;
        g.SetActive(false);
    }
}
