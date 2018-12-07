using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {
    public static LobbyManager Instance;

    public Stack<int> playerNumberStack = new Stack<int>();

    public LobbyController[] lobbyPlayers = new LobbyController[4];

    public Dictionary<int, int> playerNumbers = new Dictionary<int, int>();

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
        playerNumberStack.Push(3);
        playerNumberStack.Push(2);
        playerNumberStack.Push(1);
        playerNumberStack.Push(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        foreach(LobbyController l in lobbyPlayers)
        {
            playerNumbers.Add(l.playerNumber, l.controllerNumber);
        }

        SceneManager.LoadScene("DannyLevel");
    }
}
