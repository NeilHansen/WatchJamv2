using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : MonoBehaviour {

    public bool isBroken;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(isBroken)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            MonsterUI.Instance.ToggleMonsterInteractText(true);
        }

        if (other.gameObject.tag == "Security")
        {
            SecurityUI.Instance.TogglePlayerInteractText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            MonsterUI.Instance.ToggleMonsterInteractText(false);
        }

        if (other.gameObject.tag == "Security")
        {
            SecurityUI.Instance.TogglePlayerInteractText(false);
        }
    }
}
