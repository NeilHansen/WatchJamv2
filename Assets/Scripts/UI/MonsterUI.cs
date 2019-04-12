using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Prototype.NetworkLobby;
using Rewired;

public class MonsterUI : MonoBehaviour {
    public static MonsterUI Instance;
    public Player player;
    private int controllerNumber = 0;

    public Text interactText;

    public GameObject SeenImage;
    public GameObject Hurt;
    public Image VisibilitySlider;
    public Text livesText;

    public Image drainIcon;
    public Color drainColor;

    public Image punchIcon;
    public Color punchColor;

    public GameObject mountIcon;
    public GameObject dismountIcon;

    public GameObject GameOver;
    public GameObject MonsterWins;
    public GameObject SecurityWins;

    public TMP_Text GameTimerText;

    private int NumOfLives;

    public TMP_Text terminalText;

    private bool doOnce = true;

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
        drainColor = drainIcon.color;
        punchColor = punchIcon.color;

        VisibilitySlider.fillAmount = 1;

        NumOfLives = GameManager.Instance.MonsterNumOfLives;
        livesText.text = "Lives: " + NumOfLives;

        player = Rewired.ReInput.players.GetPlayer(controllerNumber);
    }

    void Update()
    {
        if ((GameManager.Instance.MonsterWins || GameManager.Instance.SecurityWins) && doOnce && player.GetButton("Interact"))
        {
            doOnce = false;
            ReturnToLobby();
        }

        SetGameTimerText(GameManager.Instance.GameTimer);
        livesText.text = "Lives: " + GameManager.Instance.MonsterNumOfLives;
        terminalText.text = "Terminals Broken : " + GameManager.Instance.brokenTerminalCount + "/6";
    }

    public void SetMonsterWin(bool b)
    {
        GameOver.SetActive(true);

        if (b)
        {
            MonsterWins.SetActive(true);
        }
        else
        {
            SecurityWins.SetActive(true);
        }
    }

    public void SetGameTimerText(float time)
    {
        GameTimerText.text = "Timer: " + time;
    }

    public void ResetMonsterUI()
    {
        VisibilitySlider.fillAmount = 1.0f;
    }

    //Turn on/off interaction text when hitting terminal
    public void ToggleMonsterInteractText(bool b)
    {
        interactText.enabled = b;
    }

    //Call to either see or hide the hurt screen
    public void SetMonsterHurt(bool b)
    {
        //Turn off seen imageg when alpha is 0
        Hurt.SetActive(b);
    }

    //Call to either see or hide the icon
    public void SetMonsterSeenIcon(bool b)
    {
        //Turn off seen imageg when alpha is 0
        SeenImage.SetActive(b);
    }

    public void SetVisibilitySlider(float value)
    {
        VisibilitySlider.fillAmount = value;
    }

    public void SetMountIcon(bool b)
    {
        //Keep color semi - transparent
        mountIcon.SetActive(b);
    }

    public void SetDismountIcon(bool b)
    {
        //Keep color semi - transparent
        dismountIcon.SetActive(b);
    }

    public void SetDrainIcon(float dc = 0.35f)
    {
        //Keep color semi - transparent
        drainColor.a = dc;
        drainIcon.color = drainColor;
    }

    public void SetPunchIcon(float pc = 0.35f)
    {
        //Keep color semi - transparent
        punchColor.a = pc;
        punchIcon.color = punchColor;
    }

    public void ReturnToLobby()
    {
        LobbyPlayer[] l = GameObject.FindObjectsOfType<LobbyPlayer>();

        //Reset the bool so host cant just start it right away
        foreach(LobbyPlayer lp in l)
        {
            lp.readyToBegin = false;
            lp.doOnce = true;
        }

        LobbyManager.s_Singleton.doOnce = true;
        LobbyManager.s_Singleton.SendReturnToLobby();
    }
}
