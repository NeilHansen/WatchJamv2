﻿using System.Collections;
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
    public Transform spawnPositionMonster;
    public Transform spawnPositionSecurity1;
    public Transform spawnPositionSecurity2;
    public Transform spawnPositionSecurity3;

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

    // Update is called once per frame
    void Update () {
        if(isServer && GameTimer >= 0.0f)
        {
            GameTimer += Time.deltaTime;
            if (MonsterNumOfLives <= 0)
            {
                SecurityWins = true;
                Debug.Log("Game Over Monster");
            }
        }

        if(SecurityWins)
        {
            Time.timeScale = 0.0f;
            Debug.Log("Security Wins");
        }
    }

    //Repspawn player in right position
    public void ResetMonster()
    {
        CmdMinusMonsterLife();
        localPlayer.GetComponent<MonsterController>().CmdResetAlpha();
        MonsterUI.Instance.ResetMonsterUI();
        localPlayer.transform.position = spawnPositionMonster.gameObject.transform.position;
        Debug.Log("Respawn Mon");
    }

    [Command]
    public void CmdMonsterWins()
    {
        MonsterWins = true;
    }

    [Command]
    public void CmdMinusMonsterLife()
    {
        GameManager.Instance.MonsterNumOfLives -= 1;
    }
}
