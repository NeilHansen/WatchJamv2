using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Monster")
        {
            GameManager.Instance.CmdMonsterWins();
            Time.timeScale = 0.0f;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
