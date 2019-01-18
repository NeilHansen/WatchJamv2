using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlankPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;
    [SyncVar]
    public Color playerColor;

    public GameObject gamePlayerPrefab;

    void Start()
    {
        if (isServer)
            StartCoroutine(WaitForReady());
    }

    public void SpawnUnit()
    {
        GameObject tempUnit = Instantiate(gamePlayerPrefab);
        tempUnit.GetComponent<GamePlayer>().colour = playerColor;
        tempUnit.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-3.0f, 3.0f), 0.0f);
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
