using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;
    [SyncVar]
    public Color playerColor;
    [SyncVar]
    public int playerNumber;

    public GameObject prefab_MonsterHud;
    public GameObject prefab_SecurityHud;

    public GameObject gamePlayerPrefab;

    public float monsterLightNumber = 1.0f;
    public float securityLightNumber = 0.5f;

    private GameObject tempUnit;

    void Start()
    {
        if (isServer)
            StartCoroutine(WaitForReady());

        if (isLocalPlayer)
            SpawnUI();
    }

    public void SpawnUI()
    {
        if(playerNumber == 0)
        {
            RenderSettings.ambientIntensity = monsterLightNumber;
            GameObject.Instantiate(prefab_MonsterHud);
        }
        else
        {
            RenderSettings.ambientIntensity = securityLightNumber;
            GameObject.Instantiate(prefab_SecurityHud);
        }
    }

    //Spawning the correct gameobject and giving authority over it.
    public void SpawnUnit()
    {
        tempUnit = Instantiate(gamePlayerPrefab);

        switch (playerNumber)
        {
            case 0:
                tempUnit.transform.position = GameManager.Instance.GetMonsterSpawnPosition().position;
                tempUnit.GetComponent<MonsterController>().networkPlayer = this;
                break;

            case 1:
                tempUnit.transform.position = GameManager.Instance.GetSecurity1SpawnPosition().position;
                tempUnit.GetComponent<SecurityController>().networkPlayer = this;
                break;

            case 2:
                tempUnit.transform.position = GameManager.Instance.GetSecurity2SpawnPosition().position;
                tempUnit.GetComponent<SecurityController>().networkPlayer = this;
                break;

            case 3:
                tempUnit.transform.position = GameManager.Instance.GetSecurity3SpawnPosition().position;
                tempUnit.GetComponent<SecurityController>().networkPlayer = this;
                break;
        }
        NetworkServer.SpawnWithClientAuthority(tempUnit, connectionToClient);
    }

    IEnumerator WaitForReady()
    {
        while (!connectionToClient.isReady)
        {
            yield return new WaitForSeconds(0.25f);
        }
        SpawnUnit();
    }
}
