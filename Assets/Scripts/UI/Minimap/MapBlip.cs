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

    public enum displayOnMap
    {
        none,
        security,
        monster,
        both,
    }

    public sprite spriteEnum = sprite.none;
    public displayOnMap display = displayOnMap.none;

    public Color color;

    //Map refresh timer
    private float refreshTime;
    public GameObject PlayerBlip;
    public GameObject[] MonsterBlips = new GameObject[3];

    // Use this for initialization
    void Start()
    {
        refreshTime = MapManager.Instance.refreshTimePlayer;

        CreateBlips(display);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlips(display);
    }

    private void CreateBlips(displayOnMap d)
    {
        switch (d)
        {
            case displayOnMap.security:
                SecurityCreate();
                break;
            case displayOnMap.monster:
                MonsterCreate();
                break;
            case displayOnMap.both:
                SecurityCreate();
                MonsterCreate();
                break;
        }
    }

    private void UpdateBlips(displayOnMap d)
    {
        switch(d)
        {
            case displayOnMap.security:
                refreshTime -= Time.deltaTime;
                if (refreshTime < 0)
                {
                    //Reset Time
                    refreshTime = MapManager.Instance.refreshTimePlayer;

                    //Making sure to update the blip as it moves
                    MonsterBlips[0].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    MonsterBlips[1].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    MonsterBlips[2].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    MonsterBlips[0].GetComponent<Image>().color = color;
                    MonsterBlips[1].GetComponent<Image>().color = color;
                    MonsterBlips[2].GetComponent<Image>().color = color;
                }
                break;
            case displayOnMap.monster:
                refreshTime -= Time.deltaTime;
                if (refreshTime < 0)
                {
                    //Reset Time
                    refreshTime = MapManager.Instance.refreshTimePlayer;

                    //Making sure to update the blip as it moves
                    PlayerBlip.transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    PlayerBlip.GetComponent<Image>().color = color;
                }
                break;
            case displayOnMap.both:
                refreshTime -= Time.deltaTime;
                if (refreshTime < 0)
                {
                    //Reset Time
                    refreshTime = MapManager.Instance.refreshTimePlayer;

                    //Making sure to update the blip as it moves
                    MonsterBlips[0].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    MonsterBlips[1].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    MonsterBlips[2].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Making sure to update the blip as it moves
                    PlayerBlip.transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    MonsterBlips[0].GetComponent<Image>().color = color;
                    MonsterBlips[1].GetComponent<Image>().color = color;
                    MonsterBlips[2].GetComponent<Image>().color = color;

                    PlayerBlip.GetComponent<Image>().color = color;
                }
                break;
        }
    }

    private void SecurityCreate()
    {
        switch (spriteEnum)
        {
            case sprite.character:
                for (int i = 0; i < MonsterBlips.Length; i++)
                {
                    MonsterBlips[i] = GameObject.Instantiate(MapManager.Instance.PlayerBlip);
                    MonsterBlips[i].GetComponent<Image>().color = color;
                }
                break;
            case sprite.terminal:
                for (int i = 0; i < MonsterBlips.Length; i++)
                {
                    MonsterBlips[i] = GameObject.Instantiate(MapManager.Instance.TermainalBlip);
                    MonsterBlips[i].GetComponent<Image>().color = color;
                }
                break;
            case sprite.exit:
                for (int i = 0; i < MonsterBlips.Length; i++)
                {
                    MonsterBlips[i] = GameObject.Instantiate(MapManager.Instance.ExitBlip);
                    MonsterBlips[i].GetComponent<Image>().color = color;
                }
                break;
        }

        //Set the blip gameobject parent to the map HUD
        MonsterBlips[0].transform.SetParent(MapManager.Instance.playerMap1.transform);
        MonsterBlips[1].transform.SetParent(MapManager.Instance.playerMap2.transform);
        MonsterBlips[2].transform.SetParent(MapManager.Instance.playerMap3.transform);
    }

    private void MonsterCreate()
    {
        switch (spriteEnum)
        {
            case sprite.character:
                {
                    PlayerBlip = GameObject.Instantiate(MapManager.Instance.PlayerBlip);
                    PlayerBlip.GetComponent<Image>().color = color;
                }
                break;
            case sprite.terminal:
                {
                    PlayerBlip = GameObject.Instantiate(MapManager.Instance.TermainalBlip);
                    PlayerBlip.GetComponent<Image>().color = color;
                }
                break;
            case sprite.exit:
                {
                    PlayerBlip = GameObject.Instantiate(MapManager.Instance.ExitBlip);
                    PlayerBlip.GetComponent<Image>().color = color;
                }
                break;

        }

        //Set the blip gameobject parent to the map HUD
        PlayerBlip.transform.SetParent(MapManager.Instance.monsterMap.transform);
    }
}
