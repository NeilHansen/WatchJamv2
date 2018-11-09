using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Script to assign to whatever we want to appear on the map
public class MapBlip : MonoBehaviour {

    //For access outside this script
    public GameObject Blip { get; private set; }
    public Color color;

    //Map refresh timer
    private float refreshTime;

    // Use this for initialization
    void Start()
    {
        //Set time to the global set map refresh time
        refreshTime = Map.Instance.refreshTime;

        Blip = GameObject.Instantiate(Map.Instance.BlipPrefab);
        //Set the blip gameobject parent to the map HUD
        Blip.transform.SetParent(Map.Instance.transform);

        Blip.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {

        refreshTime -= Time.deltaTime;
        if (refreshTime < 0)
        {
            //Reset Time
            refreshTime = Map.Instance.refreshTime;

            //Making sure to update the blip as it moves
            Blip.transform.position = Map.Instance.WorldPositionToMap(transform.position);
        }

    }

    void OnDestroy()
    {
        Destroy(Blip);
    }

}
