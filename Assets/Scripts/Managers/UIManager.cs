using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public List<MonsterUI> monsterUIs = new List<MonsterUI>();
    public List<PlayerUI> playerUIs = new List<PlayerUI>();

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
    public void ToggleInteractText(int playerNumber, bool b)
    {
        foreach (PlayerUI p in playerUIs)
        {
            if (p.playerNumber == playerNumber)
            {
                p.ToggleInteractText(b);
                break;
            }
            else
            {
                Debug.Log("Couldn't find player Hud that matches that ID. Or something went wrong");
            }
        }
    }
    #endregion

    #region Monster Functions
    public void MonsterSeenUI(int playerNumber, MonsterController mon, bool b)
    {
        foreach(MonsterUI m in monsterUIs)
        {
            if(m.playerNumber == playerNumber)
            {
                m.MonsterSeenUI(mon, b);
                break;
            }
            else
            {
                Debug.Log("Couldn't find player Hud that matches that ID. Or something went wrong");
            }
        }
    }

    public void MonsterDrainUI(int playerNumber, MonsterController mon)
    {
        foreach (MonsterUI m in monsterUIs)
        {
            if (m.playerNumber == playerNumber)
            {
                m.MonsterDrainUI(mon);
                break;
            }
            else
            {
                Debug.Log("Couldn't find player Hud that matches that ID. Or something went wrong");
            }
        }
    }

    public void stopDraining(int playerNumber, MonsterController mon)
    {
        foreach (MonsterUI m in monsterUIs)
        {
            if (m.playerNumber == playerNumber)
            {
                StartCoroutine(m.stopDraining(mon));
                break;
            }
            else
            {
                Debug.Log("Couldn't find player Hud that matches that ID. Or something went wrong");
            }
        }
    }

    public void stopPunching(int playerNumber, MonsterController mon)
    {
        foreach (MonsterUI m in monsterUIs)
        {
            if (m.playerNumber == playerNumber)
            {
                StartCoroutine(m.stopPunching(mon));
                break;
            }
            else
            {
                Debug.Log("Couldn't find player Hud that matches that ID. Or something went wrong");
            }
        }
    }
    #endregion
}
