using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public static GameManager Instance;

    [Header("Player References")]
    public GameObject localPlayer;

    [Header("Prefab Variables")]
    public GameObject monsterHUD;
    public GameObject playerHUD;

    [Header("Spawn Location")]
    public GameObject[] spawnPositionMonster;
    public GameObject[] spawnPositionSecurity1;
    public GameObject[] spawnPositionSecurity2;
    public GameObject[] spawnPositionSecurity3;

    public bool isMonster = false;

    [Header("Game Related Variables")]
    [SyncVar]
    public float GameTimer = 0.0f;

    [SyncVar]
    public int MonsterNumOfLives = 3;
    [SyncVar]
    public bool MonsterWins = false;
    [SyncVar]
    public bool SecurityWins = false;
    public bool reset = false;
    [SyncVar]
    public int brokenTerminalCount = 0;

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
    }

    public Transform GetMonsterSpawnPosition()
    {
        if(spawnPositionMonster.Length == 0)
        {
            spawnPositionMonster = GameObject.FindGameObjectsWithTag("MonsterSpawn");
        }

        return spawnPositionMonster[Random.Range(0, spawnPositionMonster.Length - 1)].transform;
    }

    public Transform GetSecurity1SpawnPosition()
    {
        if (spawnPositionSecurity1.Length == 0)
        {
            spawnPositionSecurity1 = GameObject.FindGameObjectsWithTag("SecuritySpawn1");
        }

        return spawnPositionSecurity1[Random.Range(0, spawnPositionSecurity1.Length - 1)].transform;
    }

    public Transform GetSecurity2SpawnPosition()
    {
        if (spawnPositionSecurity2.Length == 0)
        {
            spawnPositionSecurity2 = GameObject.FindGameObjectsWithTag("SecuritySpawn2");
        }

        return spawnPositionSecurity2[Random.Range(0, spawnPositionSecurity2.Length - 1)].transform;
    }

    public Transform GetSecurity3SpawnPosition()
    {
        if (spawnPositionSecurity3.Length == 0)
        {
            spawnPositionSecurity3 = GameObject.FindGameObjectsWithTag("SecuritySpawn3");
        }

        return spawnPositionSecurity3[Random.Range(0, spawnPositionSecurity3.Length - 1)].transform;
    }

    // Update is called once per frame
    void Update () {
        if(GameTimer >= 0.0f)
        {
            GameTimer += Time.deltaTime;
            if (MonsterNumOfLives <= 0)
            {
                SecurityWins = true;
            }
        }

        if(SecurityWins)
        {
            if (MonsterUI.Instance != null)
            {
                MonsterUI.Instance.SetMonsterWin(false);
            }

            if (SecurityUI.Instance != null)
            {
                SecurityUI.Instance.SetMonsterWin(true);
            }
        }
        else if(MonsterWins)
        {
            if (MonsterUI.Instance != null)
            {
                MonsterUI.Instance.SetMonsterWin(true);
            }

            if (SecurityUI.Instance != null)
            {
                SecurityUI.Instance.SetMonsterWin(false);
            }
        }
    }

    [Command]
    public void CmdMonsterWins()
    {
        MonsterWins = true;
    }

    public void MinusMonsterLife()
    {
        MonsterNumOfLives -= 1;
    }

    public void AddToTermainalCount()
    {
        brokenTerminalCount += 1;
    }
}
