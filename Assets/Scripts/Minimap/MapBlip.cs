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

    public Color color;

    //Map refresh timer
    private float refreshTime;
    private GameObject[] Blips = new GameObject[3];

    // Use this for initialization
    void Start()
    {
        refreshTime = MapManager.Instance.refreshTimePlayer;

        switch (spriteEnum)
        {
            case sprite.character:
                CreateBlip(MapManager.Instance.PlayerBlip);
                break;
            case sprite.terminal:
                CreateBlip(MapManager.Instance.TermainalBlip);
                break;
            case sprite.exit:
                CreateBlip(MapManager.Instance.ExitBlip);
                break;

        }

        //Set the blip gameobject parent to the map HUD
        Blips[0].transform.SetParent(MapManager.Instance.playerMap1.transform);
        Blips[1].transform.SetParent(MapManager.Instance.playerMap2.transform);
        Blips[2].transform.SetParent(MapManager.Instance.playerMap3.transform);
    }

    // Update is called once per frame
    void Update()
    {
        refreshTime -= Time.deltaTime;
        if (refreshTime < 0)
        {
            //Reset Time
            refreshTime = MapManager.Instance.refreshTimePlayer;

            //Making sure to update the blip as it moves
            Blips[0].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
            Blips[1].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
            Blips[2].transform.position = MapManager.Instance.WorldPositionToMap(transform.position);
        }

    }

    private void CreateBlip(GameObject prefab)
    {
        for (int i = 0; i < Blips.Length; i++)
        {
            Blips[i] = GameObject.Instantiate(prefab);
            Blips[i].GetComponent<Image>().color = color;
        }
    }
}
