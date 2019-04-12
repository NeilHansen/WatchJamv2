using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class LobbyTutorialScript : MonoBehaviour {

    public GameObject monsterPanel;
    public GameObject[] monsterSubPanels;
    public GameObject securityPanel;
    public GameObject[] securitySubPanels;
    public float interval = 5.0f;

    public void StartFading()
    {
        GameObject[] l = GameObject.FindGameObjectsWithTag("LobbyPlayer");

        for (int i = 0; i < l.Length; i++)
        {
            NetworkIdentity n = l[i].GetComponent<NetworkIdentity>();

            if (n.hasAuthority)
            {
                if (n.isServer)
                {
                    StartCoroutine(FadingInMonster());
                    break;
                }
                else
                {
                    StartCoroutine(FadingInSecurity());
                    break;
                }
            }
        }
    }

    public void ResetFading()
    {
        monsterPanel.SetActive(false);
        securityPanel.SetActive(false);

        foreach(GameObject g in monsterSubPanels)
        {
            g.SetActive(false);
        }

        foreach (GameObject g in securitySubPanels)
        {
            g.SetActive(false);
        }
    }

    IEnumerator FadingInSecurity()
    {
        securityPanel.SetActive(true);
        securitySubPanels[0].SetActive(true);
        StartCoroutine(securitySubPanels[0].GetComponent<TutorialVideoPlayer>().PlayVideo());
        yield return new WaitForSeconds(interval);
        securitySubPanels[1].SetActive(true);
        StartCoroutine(securitySubPanels[1].GetComponent<TutorialVideoPlayer>().PlayVideo());
        yield return new WaitForSeconds(interval);
        securitySubPanels[2].SetActive(true);
        StartCoroutine(securitySubPanels[1].GetComponent<TutorialVideoPlayer>().PlayVideo());
    }

    IEnumerator FadingInMonster()
    {
        monsterPanel.SetActive(true);
        monsterSubPanels[0].SetActive(true);
        StartCoroutine(monsterSubPanels[0].GetComponent<TutorialVideoPlayer>().PlayVideo());
        yield return new WaitForSeconds(interval);
        monsterSubPanels[1].SetActive(true);
        StartCoroutine(monsterSubPanels[1].GetComponent<TutorialVideoPlayer>().PlayVideo());
        yield return new WaitForSeconds(interval);
        monsterSubPanels[2].SetActive(true);
        StartCoroutine(monsterSubPanels[2].GetComponent<TutorialVideoPlayer>().PlayVideo());
    }

	// Update is called once per frame
	void Update () {
		
	}
}
