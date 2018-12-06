using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public static MapManager Instance;

    //Map refresh timer for player
    public float refreshTimePlayer = 1.5f;
    //Map refresh timer for monster
    public float refreshTimeMonster = 1.5f;

    public RectTransform Hud;
    //Size of the map
    public RectTransform mapRect;

    //Terrain size based on the two points
    private Vector2 terrainSize;

    //Read the two corners on the map from LayoutGen
    public Transform Corner1, Corner2;

    //For access outside this script
    public GameObject PlayerBlip;
    public GameObject TermainalBlip;
    public GameObject ExitBlip;

    public GameObject playerMap1;
    public GameObject playerMap2;
    public GameObject playerMap3;
    public GameObject monsterMap;

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

        //Calculating the terrain size based on the two corners
        terrainSize = new Vector2(
            MapManager.Instance.Corner2.position.x - MapManager.Instance.Corner1.position.x,
            MapManager.Instance.Corner2.position.z - MapManager.Instance.Corner1.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Calcuating my world position relative to the map
    public Vector2 WorldPositionToMap(Vector3 point)
    {
        Vector2 mapPos;
        //var pos = point - Corner1.position;

        mapPos = new Vector2(
            (point.x / terrainSize.x * mapRect.rect.width * Hud.localScale.x),
            (point.z / terrainSize.y * mapRect.rect.height * Hud.localScale.x));

        return mapPos;
    }
}
