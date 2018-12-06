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
        exit,
    }

    public sprite spriteEnum = sprite.none;

    public Color color;

    //Map refresh timer
    private float refreshTime;
    private GameObject Blip;

    // Use this for initialization
    void Start()
    {
        //Set time to the global set map refresh time
        refreshTime = MapManager.Instance.refreshTimeMonster;

        switch (spriteEnum)
        {
            case sprite.character:
                Blip = GameObject.Instantiate(MapManager.Instance.PlayerBlip);
                break;
            case sprite.terminal:
                Blip = GameObject.Instantiate(MapManager.Instance.TermainalBlip);
                break;
            case sprite.exit:
                Blip = GameObject.Instantiate(MapManager.Instance.ExitBlip);
                break;

        }

        //Set the blip gameobject parent to the map HUD
        Blip.transform.SetParent(MapManager.Instance.monsterMap.transform);

        Blip.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        refreshTime -= Time.deltaTime;
        if (refreshTime < 0)
        {
            //Reset Time
            refreshTime = MapManager.Instance.refreshTimeMonster;

            //Making sure to update the blip as it moves
            Blip.GetComponent<RectTransform>().anchoredPosition = MapManager.Instance.WorldPositionToMap(transform.position);
            Blip.GetComponent<Image>().color = color;
        }

    }

    void OnDestroy()
    {
        Destroy(Blip);
    }

}
