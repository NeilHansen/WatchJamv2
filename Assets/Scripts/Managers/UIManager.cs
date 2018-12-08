using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public Dictionary<int, MonsterUI> monsterUIsDictionary = new Dictionary<int, MonsterUI>();
    public Dictionary<int, PlayerUI> playerUIsDictionary = new Dictionary<int, PlayerUI>();

    public GameObject[] slots = new GameObject[4];

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

    //Quick way to set slots based on lobby controller click
    public void SetLobbySlot(int playerN, int controllerN)
    {
        slots[playerN].GetComponent<Image>().color = Color.green;
        slots[playerN].transform.GetChild(0).GetComponent<Text>().text = "Controller " + controllerN + " Ready";
    }

    #region Player Functions
    //Set the value for the flashlight slider
    public void SetFlashUIValue(int playerNumber, float value)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.SetFlashUIValue(value);
    }

    //Set the max value for the flashlight slider
    public void SetFlashUIMaxValue(int playerNumber, float value)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.SetFlashUIMaxValue(value);
    }

    //Turn on off the interact text
    public void TogglePlayerInteractText(int playerNumber, bool b)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.TogglePlayerInteractText(b);
    }

    //Turn on off the stunned text
    public void ToggleStunnedText(int playerNumber, bool b)
    {
        PlayerUI temp = playerUIsDictionary[playerNumber];
        temp.ToggleStunnedText(b);
    }
    #endregion

    #region Monster Functions
    //Turn on off monster interact text
    public void ToggleMonsterInteractText(int playerNumber, bool b)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        temp.ToggleMonsterInteractText(b);
    }

    //Turn on off monster seen icon
    public void MonsterSeenUI(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        temp.MonsterSeenUI(mon);
    }

    //Lower monster visibility slider when draining
    public void MonsterDrainUI(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        temp.MonsterDrainUI(mon);
    }

    //Affect monster drain icon based on values set in Monster Controller
    public void stopDraining(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        StartCoroutine(temp.stopDraining(mon));
    }

    //Affect monster punch icon based on values set in Monster Controller
    public void stopPunching(int playerNumber, MonsterController mon)
    {
        MonsterUI temp = monsterUIsDictionary[playerNumber];
        StartCoroutine(temp.stopPunching(mon));
    }
    #endregion
}
