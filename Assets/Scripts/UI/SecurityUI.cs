using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecurityUI : MonoBehaviour {
    public static SecurityUI Instance;

    public Text interact;
    public Text stunnedText;

    public Color flashlightUse;
    public Slider flashUI;
    public Image flashlight;
    public GameObject[] dots;

    public TMP_Text GameTimerText;

    public GameObject Flashlightoverheat;
    public GameObject GameOver;
    public GameObject MonsterWins;
    public GameObject SecurityWins;

    public GameObject ping;
    public GameObject sprint;

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

    public void SetFlashLightUse(bool b)
    {
        if(b)
        {
            flashlight.color = flashlightUse;
            foreach (GameObject i in dots)
            {
                i.GetComponent<Image>().color = flashlightUse;
            }
        }
        else
        {
            flashlight.color = Color.white;
            foreach (GameObject i in dots)
            {
                i.GetComponent<Image>().color = Color.white;
            }
        }   
    }

    public void SetOverHeatIcon(bool b)
    {
        if(b)
        {
            flashlight.color = Color.red;
            foreach(GameObject i in dots)
            {
                i.GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            flashlight.color = Color.white;
            foreach (GameObject i in dots)
            {
                i.GetComponent<Image>().color = Color.white;
            }
        }

        //Turn off seen imageg when alpha is 0
        Flashlightoverheat.SetActive(b);
    }

    public void SetSprint(bool b)
    {
        sprint.SetActive(b);
    }

    public void SetPing(bool b)
    {
        ping.SetActive(b);
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
