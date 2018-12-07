using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [Header("Player References")]
    public GameObject monsterGameObject;
    public MonsterController monsterController;

    public List<GameObject> players = new List<GameObject>();
    public List<PlayerController> playerControllers = new List<PlayerController>();

    [Header("Prefab Variables")]
    public GameObject monsterPrefab;
    public GameObject playerPrefab;

    public GameObject monsterHUD;
    public GameObject playerHUD;

    [Header("Spawn Locations")]
    public Transform monsterSpawnLocation;
    public Transform playerSpawnLocation1;
    public Transform playerSpawnLocation2;
    public Transform playerSpawnLocation3;

    [Header("Init Variables")]
    public GameObject interactables;

    [Header("Game Related Variables")]
    public float WinTimer = 5.0f;
    public int MonsterNumOfLives = 1;

    public bool reset = false;

    // Use this for initialization
    void Awake () {
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

    void Start()
    {
        SetupGame();
    }
	
	// Update is called once per frame
	void Update () {
        //GameOver
        if (MonsterNumOfLives <= 0)
        {
            Time.timeScale = 0.0f;
        }
    }

    //Setup UI before player spawn to set all variables
    public void SetupGame()
    {
        SpawnUI();
        SpawnPlayers();
        interactables.SetActive(true);
    }

    //Spawn all players including monster
    public void SpawnPlayers()
    {
        monsterGameObject = GameObject.Instantiate(monsterPrefab, monsterSpawnLocation.position, monsterSpawnLocation.rotation);
        monsterController = monsterGameObject.GetComponent<MonsterController>();

        players.Add(GameObject.Instantiate(playerPrefab, playerSpawnLocation1.position, playerSpawnLocation1.rotation));
        players.Add(GameObject.Instantiate(playerPrefab, playerSpawnLocation2.position, playerSpawnLocation2.rotation));
        players.Add(GameObject.Instantiate(playerPrefab, playerSpawnLocation3.position, playerSpawnLocation3.rotation));

        for(int i = 0; i < players.Count; i++)
        {
            playerControllers.Add(players[i].GetComponent<PlayerController>());
        }

        //Init controllers to set display and variables
        monsterController.Init(0, LobbyManager.Instance.playerNumbers[0]);
        playerControllers[0].Init(1, LobbyManager.Instance.playerNumbers[1]);
        playerControllers[1].Init(2, LobbyManager.Instance.playerNumbers[2]);
        playerControllers[2].Init(3, LobbyManager.Instance.playerNumbers[3]);
    }

    //Setup UI
    public void SpawnUI()
    {
        GameObject hud1 = GameObject.Instantiate(monsterHUD, UIManager.Instance.transform);
        GameObject hud2 = GameObject.Instantiate(playerHUD, UIManager.Instance.transform);
        GameObject hud3 = GameObject.Instantiate(playerHUD, UIManager.Instance.transform);
        GameObject hud4 = GameObject.Instantiate(playerHUD, UIManager.Instance.transform);

        //Init Hud to set display and variables
        hud1.GetComponent<MonsterUI>().InitUI(0);
        hud2.GetComponent<PlayerUI>().InitUI(1);
        hud3.GetComponent<PlayerUI>().InitUI(2);
        hud4.GetComponent<PlayerUI>().InitUI(3);

        //Settings map manager variables that are in the scene
        MapManager.Instance.Hud = hud1.GetComponent<RectTransform>();
        MapManager.Instance.mapRect = hud1.transform.GetChild(1).gameObject.GetComponent<RectTransform>();

        MapManager.Instance.monsterMap = hud1.transform.GetChild(1).gameObject;
        MapManager.Instance.playerMap1 = hud2.transform.GetChild(1).gameObject;
        MapManager.Instance.playerMap2 = hud3.transform.GetChild(1).gameObject;
        MapManager.Instance.playerMap3 = hud4.transform.GetChild(1).gameObject;

        //Set the terrain size so minimap works
        MapManager.Instance.SetTerrainSize();
    }

    public void Reset()
    {
        monsterGameObject.transform.position = monsterSpawnLocation.gameObject.transform.position;
        Debug.Log("Respawn");
    }
}
