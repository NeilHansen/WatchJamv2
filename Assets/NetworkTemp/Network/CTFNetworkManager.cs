using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CTFNetworkManager : NetworkLobbyManager
{

    static public CTFNetworkManager Singleton;

    public RectTransform playerPanel;
    public RectTransform howToPlayPanel;

    [HideInInspector]
    public int _numPlayers;


    private void Start()
    {
        Singleton = this;
    }

    public void AddPlayer(RectTransform rectTransform)
    {
        rectTransform.SetParent(GameObject.FindWithTag("Canvas").transform.GetChild(0).transform, false);
        rectTransform.anchoredPosition = new Vector2(-600 + _numPlayers * 400, 50);
        _numPlayers += 1;
        howToPlayPanel.gameObject.SetActive(false);
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        _numPlayers -= 1;
        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            CTFLobbyPlayer p = lobbySlots[i] as CTFLobbyPlayer;

            if (p != null)
            {
                p.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600 + i * 400, 50);
            }
        }
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        CTFLobbyPlayer cc = lobbyPlayer.GetComponent<CTFLobbyPlayer>();
        BlankPlayer player = gamePlayer.GetComponent<BlankPlayer>();

        player.playerName = cc.playerName;
        player.playerColor = cc.playerColor;
        if (cc.playerNum == 1)
        {
            player.gamePlayerPrefab = spawnPrefabs[0];
        }
        else
        {
            player.gamePlayerPrefab = spawnPrefabs[1];
        }

        gamePlayer.transform.position = Vector3.zero;

        return true;
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            playerPanel.gameObject.SetActive(true);
        }
        else
        {
            playerPanel.gameObject.SetActive(false);
        }
    }
}
