using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public Dictionary<int, MonsterUI> monsterUIsDictionary = new Dictionary<int, MonsterUI>();
    public Dictionary<int, PlayerUI> playerUIsDictionary = new Dictionary<int, PlayerUI>();

    // Use this for initialization
    void Awake()
    {
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #region Player Functions
    public void SetFlashUIValue(int playerNumber, float value)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.SetFlashUIValue(value);
    }

    public void SetFlashUIMaxValue(int playerNumber, float value)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.SetFlashUIMaxValue(value);
    }

    public void TogglePlayerInteractText(int playerNumber, bool b)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.TogglePlayerInteractText(b);
    }

    public void ToggleStunnedText(int playerNumber, bool b)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.ToggleStunnedText(b);
    }
    #endregion

    #region Monster Functions
    public void ToggleMonsterInteractText(int playerNumber, bool b)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        temp.ToggleMonsterInteractText(b);
    }

    public void MonsterSeenUI(int playerNumber, MonsterController mon, bool b)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        temp.MonsterSeenUI(mon, b);
    }

    public void MonsterDrainUI(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        temp.MonsterDrainUI(mon);
    }

    public void stopDraining(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        StartCoroutine(temp.stopDraining(mon));
    }

    public void stopPunching(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        StartCoroutine(temp.stopPunching(mon));
    }
    #endregion
}
