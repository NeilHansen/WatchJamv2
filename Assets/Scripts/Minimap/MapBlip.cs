using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Script to assign to whatever we want to appear on the map
public class MapBlip : MonoBehaviour {

    public enum sprite
    {
        none,
        character,
        terminal,
        exit,
    }

    public sprite spriteEnum = sprite.none;

    //For access outside this script
    public GameObject Blip1 { get; private set; }
    public GameObject Blip2 { get; private set; }
    public GameObject Blip3 { get; private set; }

    public Color color;
    public Map map1;
    public Map map2;
    public Map map3;


    //Map refresh timer
    private float refreshTime;

    // Use this for initialization
    void Start()
    {
        //Set time to the global set map refresh time
        refreshTime = map1.refreshTime;
        refreshTime = map2.refreshTime;
        refreshTime = map3.refreshTime;

        switch (spriteEnum)
        {
            case sprite.character:
                Blip1 = GameObject.Instantiate(map1.BlipPrefab);
                Blip2 = GameObject.Instantiate(map2.BlipPrefab);
                Blip3 = GameObject.Instantiate(map3.BlipPrefab);
                break;
            case sprite.terminal:
                Blip1 = GameObject.Instantiate(map1.TermainalBlip);
                Blip2 = GameObject.Instantiate(map2.TermainalBlip);
                Blip3 = GameObject.Instantiate(map3.TermainalBlip);
                break;
            case sprite.exit:
                Blip1 = GameObject.Instantiate(map1.ExitBlip);
                Blip2 = GameObject.Instantiate(map2.ExitBlip);
                Blip3 = GameObject.Instantiate(map3.ExitBlip);
                break;

        }

        //Set the blip gameobject parent to the map HUD
        Blip1.transform.SetParent(map1.transform.GetChild(0));
        Blip2.transform.SetParent(map2.transform.GetChild(0));
        Blip3.transform.SetParent(map3.transform.GetChild(0));

        Blip1.GetComponent<Image>().color = color;
        Blip2.GetComponent<Image>().color = color;
        Blip3.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {

        refreshTime -= Time.deltaTime;
        if (refreshTime < 0)
        {
            //Reset Time
            refreshTime = map1.refreshTime;
            refreshTime = map2.refreshTime;
            refreshTime = map3.refreshTime;

            //Making sure to update the blip as it moves
            Blip1.transform.position = map1.WorldPositionToMap(transform.position);
            Blip2.transform.position = map2.WorldPositionToMap(transform.position);
            Blip3.transform.position = map3.WorldPositionToMap(transform.position);
        }

    }

    void OnDestroy()
    {
        Destroy(Blip1);
        Destroy(Blip2);
        Destroy(Blip3);
    }

}
