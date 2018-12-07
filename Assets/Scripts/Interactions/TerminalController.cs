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
            UIManager.Instance.ToggleMonsterInteractText(other.GetComponent<MonsterController>().playerNumber, true);
        }

        if (other.gameObject.tag == "Security")
        {
            UIManager.Instance.TogglePlayerInteractText(other.GetComponent<MonsterController>().playerNumber, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            UIManager.Instance.ToggleMonsterInteractText(other.GetComponent<MonsterController>().playerNumber, false);
        }

        if (other.gameObject.tag == "Security")
        {
            UIManager.Instance.TogglePlayerInteractText(other.GetComponent<MonsterController>().playerNumber, false);
        }
    }
}
