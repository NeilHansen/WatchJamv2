using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public GameObject monsterGameObject;
    public MonsterController monsterController;

    public List<GameObject> players = new List<GameObject>();

    public List<PlayerController> playerControllers = new List<PlayerController>();

    public GameObject spawnLocation;
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
        monsterGameObject.transform.position = spawnLocation.gameObject.transform.position;
        Debug.Log("Respawn");
    }
}
