using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Script to assign to whatever we want to appear on the map
public class MapBlipMonster : MonoBehaviour {

    public enum sprite
    {
        none,
        character,
        terminal,
    }

    public sprite spriteEnum = sprite.none;

    //For access outside this script
    public GameObject Blip { get; private set; }

    public Color color;
    public MapMonster map;


    //Map refresh timer
    private float refreshTime;

    // Use this for initialization
    void Start()
    {
        //Set time to the global set map refresh time
        refreshTime = map.refreshTime;

        switch (spriteEnum)
        {
            case sprite.character:
                Blip = GameObject.Instantiate(map.BlipPrefab);
                break;
            case sprite.terminal:
                Blip = GameObject.Instantiate(map.TermainalBlip);
                break;

        }

        //Set the blip gameobject parent to the map HUD
        Blip.transform.SetParent(map.transform.GetChild(0));

        Blip.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {

        refreshTime -= Time.deltaTime;
        if (refreshTime < 0)
        {
            //Reset Time
            refreshTime = map.refreshTime;

            //Making sure to update the blip as it moves
            Blip.transform.position = map.WorldPositionToMap(transform.position);
        }

    }

    void OnDestroy()
    {
        Destroy(Blip);
    }

}
