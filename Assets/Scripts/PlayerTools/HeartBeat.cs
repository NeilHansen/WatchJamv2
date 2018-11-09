using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour {

    public GameObject heartBeat;

	// Use this for initialization
	void Start () {
        heartBeat.SetActive(false);
    }
}
