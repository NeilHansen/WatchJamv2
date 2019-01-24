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
    public Transform spawnPosition;

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
            GameObject.Instantiate(prefab_MonsterHud);
        }
        else
        {
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
                spawnPosition = GameObject.FindGameObjectWithTag("MonsterSpawn").transform;
                tempUnit.transform.position = spawnPosition.position;
                GameManager.Instance.spawnPosition = spawnPosition;
                break;

            case 1:
                spawnPosition = GameObject.FindGameObjectWithTag("SecuritySpawn1").transform;
                tempUnit.transform.position = spawnPosition.position;
                break;

            case 2:
                spawnPosition = GameObject.FindGameObjectWithTag("SecuritySpawn2").transform;
                tempUnit.transform.position = spawnPosition.position;
                break;

            case 3:
                spawnPosition = GameObject.FindGameObjectWithTag("SecuritySpawn3").transform;
                tempUnit.transform.position = spawnPosition.position;
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
