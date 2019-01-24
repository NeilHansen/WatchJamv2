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

    void Start()
    {
        if (isLocalPlayer)
            Init();

        if (isServer)
            StartCoroutine(WaitForReady());
    }

    //Get Spawn Location and Spawn UI
    private void Init()
    {
        switch (playerNumber)
        {
            case 0:
                spawnPosition = GameObject.FindGameObjectWithTag("MonsterSpawn").transform;
                GameManager.Instance.spawnPosition = spawnPosition;
                GameObject.Instantiate(prefab_MonsterHud);
                break;

            case 1:
                spawnPosition = GameObject.FindGameObjectWithTag("SecuritySpawn1").transform;
                GameObject.Instantiate(prefab_SecurityHud);
                break;

            case 2:
                spawnPosition = GameObject.FindGameObjectWithTag("SecuritySpawn2").transform;
                GameObject.Instantiate(prefab_SecurityHud);
                break;

            case 3:
                spawnPosition = GameObject.FindGameObjectWithTag("SecuritySpawn3").transform;
                GameObject.Instantiate(prefab_SecurityHud);
                break;
        }
    }

    //Spawning the correct gameobject and giving authority over it.
    public void SpawnUnit()
    {
        GameObject tempUnit = Instantiate(gamePlayerPrefab);
        tempUnit.transform.position = spawnPosition.position;
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
