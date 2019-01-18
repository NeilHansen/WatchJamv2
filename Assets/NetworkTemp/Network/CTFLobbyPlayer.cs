using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class CTFLobbyPlayer : NetworkLobbyPlayer
{

    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    //used on server to avoid assigning the same color to two player
    static List<int> _colorInUse = new List<int>();

    [SyncVar(hook = "OnNameChange")]
    public string playerName = "";
    [SyncVar(hook = "OnColorChange")]
    public Color playerColor = Color.white;

    public int playerNum;

    public Image colorImage;
    public InputField nameInput;
    public Button readyButton;

    bool firstTime = true;

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        if (firstTime)
        {
            CTFNetworkManager.Singleton.AddPlayer(GetComponent<RectTransform>());
            playerNum = CTFNetworkManager.Singleton._numPlayers;
            firstTime = false;
        }

        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
    }

    void SetupOtherPlayer()
    {
        colorImage.GetComponent<EventTrigger>().enabled = false;
        colorImage.color = playerColor;

        nameInput.interactable = false;
        nameInput.text = playerName;

        readyButton.transform.GetChild(0).GetComponent<Text>().text = "Not Ready";
        readyButton.interactable = false;

        OnClientReady(readyToBegin);
    }

    void SetupLocalPlayer()
    {
        colorImage.GetComponent<EventTrigger>().enabled = true;

        if (playerColor == Color.white)
            CmdColorChange();

        nameInput.interactable = true;
        nameInput.text = playerName;

        if (playerName == "")
            CmdNameChanged("Player" + CTFNetworkManager.Singleton._numPlayers);

        readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
        readyButton.interactable = true;
    }

    public override void OnClientReady(bool readyState)
    {
        base.OnClientReady(readyState);

        if (readyState)
        {
            Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
            textComponent.text = "READY";
            readyButton.interactable = false;
            colorImage.GetComponent<EventTrigger>().enabled = false;
            nameInput.interactable = false;
        }
        else
        {
            Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
            textComponent.text = isLocalPlayer ? "JOIN" : "...";
            readyButton.interactable = isLocalPlayer;
            colorImage.GetComponent<EventTrigger>().enabled = isLocalPlayer;
            nameInput.interactable = isLocalPlayer;
        }
    }

    public void OnNameChange(string newName)
    {
        playerName = newName;
        nameInput.text = playerName;
    }

    public void OnColorChange(Color newColor)
    {
        playerColor = newColor;
        colorImage.color = newColor;
    }

    public void ChangeName(string n)
    {
        CmdNameChanged(n);
    }

    public void ChangeColor()
    {
        CmdColorChange();
    }

    public void ClickReady()
    {
        SendReadyToBeginMessage();
    }

    [Command]
    public void CmdNameChanged(string name)
    {
        playerName = name;
    }

    [Command]
    public void CmdColorChange()
    {

        int idx = System.Array.IndexOf(Colors, playerColor);

        int inUseIdx = _colorInUse.IndexOf(idx);

        if (idx < 0) idx = 0;

        idx = (idx + 1) % Colors.Length;

        bool alreadyInUse = false;

        do
        {
            alreadyInUse = false;
            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    alreadyInUse = true;
                    idx = (idx + 1) % Colors.Length;
                }
            }
        }
        while (alreadyInUse);

        if (inUseIdx >= 0)
        {//if we already add an entry in the colorTabs, we change it
            _colorInUse[inUseIdx] = idx;
        }
        else
        {//else we add it
            _colorInUse.Add(idx);
        }

        playerColor = Colors[idx];
    }

}
