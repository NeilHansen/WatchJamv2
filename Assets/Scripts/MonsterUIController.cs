using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIController : MonoBehaviour {

    private float SavedUseTime = 5.0f;
    public bool isSeen;

    public Slider MonsterUI;
    public GameObject SeenUI;
    public float UIValue;

    public GameObject spawn;

    private Material monsterMaterial;
    private float materialAlpha;

    public float materialAlphaFadeRate;

    public bool isDraining;

    private Color monsterColor;

    public int MaxLives;
    private int NumOfLives;

    public Text livesText;
    // Use this for initialization
    void Start () {
       // MonsterUI.value = UIValue;
         monsterMaterial = GameObject.FindGameObjectWithTag("Monster Material").GetComponent<SkinnedMeshRenderer>().material;
        monsterMaterial.color = monsterColor;
        MonsterUI.maxValue = UIValue;
        NumOfLives = MaxLives;
    }
	
	// Update is called once per frame
	void Update () {
        monsterMaterial.color = monsterColor;
        livesText.text = "Lives: " + NumOfLives; 
        
        if (isSeen)
        {
           
            MonsterUI.value += Time.deltaTime;
            monsterColor.a += materialAlphaFadeRate * Time.deltaTime;
            SeenUI.SetActive(true);
        }
        else if(isDraining)
        {
            MonsterUI.maxValue = SavedUseTime;
            MonsterUI.value -= Time.deltaTime;
            if(monsterColor.a > 0.0 )
            monsterColor.a -= materialAlphaFadeRate * Time.deltaTime;
        }

        if (MonsterUI.value == MonsterUI.maxValue)
        {
            monsterColor.a = 0.0f;
            UIValue = 0.0f;
            this.transform.position = spawn.gameObject.transform.position;
            Debug.Log("Respawn");
            NumOfLives -= 1;
            MonsterUI.value = 0.0f;
        }

        if(NumOfLives <=0 )
        {
            Time.timeScale = 0.0f;
        }

        if(!isSeen)
        {
            SeenUI.SetActive(false);
        }
        
	}

    public void SwitchDirection()
    {
        if(isSeen)
        {
            isSeen = false;
        }
        else
        {
            isSeen = true;
        }
    }
}
