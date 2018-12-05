using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to handle the minimap
public class Map : MonoBehaviour {

    //Map refresh timer
    public float refreshTime = 1.5f;

    //Read the two corners on the map from LayoutGen
    public Transform Corner1, Corner2;
    //Little icon to spawn on the map
    public GameObject BlipPrefab;
    public GameObject TermainalBlip;
    public GameObject ExitBlip;

    //Terrain size based on the two points
    private Vector2 terrainSize;

    //Size of the map
    public RectTransform mapRect;

    //For reference in other scripts
    public static Map Instance;

    // Use this for initialization
    void Awake()
    {
        //// Singleton logic:
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    //Destroy(gameObject);
        //    return;
        //}

        //Calculating the terrain size based on the two corners
        terrainSize = new Vector2(
            Corner2.position.x - Corner1.position.x,
            Corner2.position.z - Corner1.position.z);

        //mapRect = GetComponent<RectTransform>();
    }

    //Calcuating my world position relative to the map
    public Vector2 WorldPositionToMap(Vector3 point)
    {
        Vector2 mapPos;
        //var pos = point - Corner1.position;
        if (mapRect.anchoredPosition.y > 0 && mapRect.anchoredPosition.x > 0)
        {
            mapPos = new Vector2(
            (point.x / terrainSize.x * mapRect.rect.width),
            (point.z / terrainSize.y * mapRect.rect.height)) + new Vector2((mapRect.rect.width / 4) + mapRect.anchoredPosition.x, ((mapRect.rect.height / 4) + mapRect.anchoredPosition.y));
        }
        else if(mapRect.anchoredPosition.x > 0)
        {
            mapPos = new Vector2(
            (point.x / terrainSize.x * mapRect.rect.width),
            (point.z / terrainSize.y * mapRect.rect.height)) + new Vector2((mapRect.rect.width / 4) + mapRect.anchoredPosition.x, 0);
        }
        else if (mapRect.anchoredPosition.y > 0)
        {
            mapPos = new Vector2(
            (point.x / terrainSize.x * mapRect.rect.width),
            (point.z / terrainSize.y * mapRect.rect.height)) + new Vector2(0, ((mapRect.rect.height / 4) + mapRect.anchoredPosition.y));
        }
        else
        {
            mapPos = new Vector2(
            (point.x / terrainSize.x * mapRect.rect.width),
            (point.z / terrainSize.y * mapRect.rect.height));
        }

        return mapPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
