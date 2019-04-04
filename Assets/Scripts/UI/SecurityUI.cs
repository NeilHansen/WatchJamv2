using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecurityUI : MonoBehaviour {
    public static SecurityUI Instance;

    public Text interact;
    public Text stunnedText;

    public Slider flashUI;

    public TMP_Text GameTimerText;

    public GameObject GameOver;
    public GameObject MonsterWins;
    public GameObject SecurityWins;

    public TMP_Text terminalText;

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

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        SetGameTimerText(GameManager.Instance.GameTimer);
        terminalText.text = "Terminals Broken : " + GameManager.Instance.brokenTerminalCount + "/6"; 
    }

    public void SetMonsterWin(bool b)
    {
        GameOver.SetActive(true);

        if (b)
        {
            SecurityWins.SetActive(true);
        }
        else
        {
            MonsterWins.SetActive(true);
        }
    }

    public void SetGameTimerText(float time)
    {
        GameTimerText.text = "Timer: " + time;
    }

    public void SetFlashUIValue(float value)
    {
        flashUI.value = value;
    }

    public void SetFlashUIMaxValue(float value)
    {
        flashUI.maxValue = value;
    }

    public void TogglePlayerInteractText(bool b)
    {
        interact.enabled = b;
    }

    public void ToggleStunnedText(bool b)
    {
        stunnedText.enabled = b;
    }
}
