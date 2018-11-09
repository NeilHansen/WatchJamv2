using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to handle the minimap
public class Map : MonoBehaviour {

    //Map refresh timer
    public float refreshTime = 1.0f;

    //Read the two corners on the map from LayoutGen
    public Transform Corner1, Corner2;
    //Little icon to spawn on the map
    public GameObject BlipPrefab;

    //Terrain size based on the two points
    private Vector2 terrainSize;

    //Size of the map
    private RectTransform mapRect;

    //For reference in other scripts
    public static Map Instance;

    // Use this for initialization
    void Start()
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

        //Calculating the terrain size based on the two corners
        terrainSize = new Vector2(
            Corner2.position.x - Corner1.position.x,
            Corner2.position.z - Corner1.position.z);

        mapRect = GetComponent<RectTransform>();
    }

    //Calcuating my world position relative to the map
    public Vector2 WorldPositionToMap(Vector3 point)
    {
        //var pos = point - Corner1.position;
        var mapPos = new Vector2(
            point.x / terrainSize.x * mapRect.rect.width,
            point.z / terrainSize.y * mapRect.rect.height);
        return mapPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
