using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [Header("Player References")]
    public GameObject localPlayer;

    [Header("Prefab Variables")]
    public GameObject monsterHUD;
    public GameObject playerHUD;

    [Header("Spawn Location")]
    public Transform spawnPosition;

    [Header("Game Related Variables")]
    public int MonsterNumOfLives = 3;
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
        //GameOver
        if (MonsterNumOfLives <= 0)
        {
            Time.timeScale = 0.0f;
        }
    }

    public void Reset()
    {
        localPlayer.transform.position = spawnPosition.gameObject.transform.position;
        Debug.Log("Respawn");
    }
}
