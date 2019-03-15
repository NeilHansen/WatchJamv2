using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRemoveUIandManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        if (MonsterUI.Instance != null)
        {
            Destroy(MonsterUI.Instance.gameObject);
        }

        if (SecurityUI.Instance != null)
        {
            Destroy(SecurityUI.Instance.gameObject);
        }
    }
}
