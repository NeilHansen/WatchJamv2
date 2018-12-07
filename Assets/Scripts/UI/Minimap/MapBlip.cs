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
    private float refreshTimeMonster;
    private float refreshTimePlayer;

    public GameObject MonsterBlips;
    public GameObject[] PlayerBlips = new GameObject[3];

    // Use this for initialization
    void Start()
    {
        refreshTimeMonster = MapManager.Instance.refreshTimeMonster;
        refreshTimePlayer = MapManager.Instance.refreshTimePlayer;

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
                refreshTimePlayer -= Time.deltaTime;
                if (refreshTimePlayer < 0)
                {
                    //Reset Time
                    refreshTimePlayer = MapManager.Instance.refreshTimePlayer;

                    //Making sure to update the blip as it moves
                    PlayerBlips[0].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    PlayerBlips[1].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    PlayerBlips[2].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    PlayerBlips[0].GetComponent<Image>().color = color;
                    PlayerBlips[1].GetComponent<Image>().color = color;
                    PlayerBlips[2].GetComponent<Image>().color = color;
                }
                break;
            case displayOnMap.monster:
                refreshTimeMonster -= Time.deltaTime;
                if (refreshTimeMonster < 0)
                {
                    //Reset Time
                    refreshTimeMonster = MapManager.Instance.refreshTimePlayer;

                    //Making sure to update the blip as it moves
                    MonsterBlips.transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    MonsterBlips.GetComponent<Image>().color = color;
                }
                break;
            case displayOnMap.both:
                refreshTimePlayer -= Time.deltaTime;
                refreshTimeMonster -= Time.deltaTime;
                if (refreshTimePlayer < 0)
                {
                    //Reset Time
                    refreshTimePlayer = MapManager.Instance.refreshTimePlayer;

                    //Making sure to update the blip as it moves
                    PlayerBlips[0].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    PlayerBlips[1].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
                    PlayerBlips[2].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    PlayerBlips[0].GetComponent<Image>().color = color;
                    PlayerBlips[1].GetComponent<Image>().color = color;
                    PlayerBlips[2].GetComponent<Image>().color = color;
                }
                if(refreshTimeMonster < 0)
                {
                    //Making sure to update the blip as it moves
                    MonsterBlips.transform.position = MapManager.Instance.WorldPositionToMap(transform.position);

                    //Set Colour if it ever changes
                    MonsterBlips.GetComponent<Image>().color = color;
                }
                break;
        }
    }

    private void SecurityCreate()
    {
        switch (spriteEnum)
        {
            case sprite.character:
                for (int i = 0; i < PlayerBlips.Length; i++)
                {
                    PlayerBlips[i] = GameObject.Instantiate(MapManager.Instance.PlayerBlip);
                    PlayerBlips[i].GetComponent<Image>().color = color;
                }
                break;
            case sprite.terminal:
                for (int i = 0; i < PlayerBlips.Length; i++)
                {
                    PlayerBlips[i] = GameObject.Instantiate(MapManager.Instance.TermainalBlip);
                    PlayerBlips[i].GetComponent<Image>().color = color;
                }
                break;
            case sprite.exit:
                for (int i = 0; i < PlayerBlips.Length; i++)
                {
                    PlayerBlips[i] = GameObject.Instantiate(MapManager.Instance.ExitBlip);
                    PlayerBlips[i].GetComponent<Image>().color = color;
                }
                break;
        }

        //Set the blip gameobject parent to the map HUD
        PlayerBlips[0].transform.SetParent(MapManager.Instance.playerMap1.transform);
        PlayerBlips[1].transform.SetParent(MapManager.Instance.playerMap2.transform);
        PlayerBlips[2].transform.SetParent(MapManager.Instance.playerMap3.transform);
    }

    private void MonsterCreate()
    {
        switch (spriteEnum)
        {
            case sprite.character:
                {
                    MonsterBlips = GameObject.Instantiate(MapManager.Instance.PlayerBlip);
                    MonsterBlips.GetComponent<Image>().color = color;
                }
                break;
            case sprite.terminal:
                {
                    MonsterBlips = GameObject.Instantiate(MapManager.Instance.TermainalBlip);
                    MonsterBlips.GetComponent<Image>().color = color;
                }
                break;
            case sprite.exit:
                {
                    MonsterBlips = GameObject.Instantiate(MapManager.Instance.ExitBlip);
                    MonsterBlips.GetComponent<Image>().color = color;
                }
                break;

        }

        //Set the blip gameobject parent to the map HUD
        MonsterBlips.transform.SetParent(MapManager.Instance.monsterMap.transform);
    }
}
