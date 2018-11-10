using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Trail : MonoBehaviour {

    // Use this for initialization

        
    private void Awake()
    {

        Exit exit = FindObjectOfType<Exit>();

        if (exit != null)
        {
            NavMeshAgent meshAgent = GetComponent<NavMeshAgent>();
            meshAgent.SetDestination(exit.transform.position);
           // meshAgent.
        }

    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
