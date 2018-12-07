using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour {
    public int playerNumber;

    public Text interactText;

    public Image SeenImage;
    public Slider VisibilitySlider;
    public Text livesText;

    public Image drainIcon;
    public Color drainColor;

    public Image punchIcon;
    public Color punchColor;

    private int NumOfLives;

    // Use this for initialization
    void Start () {
        drainColor = drainIcon.color;
        punchColor = punchIcon.color;

        VisibilitySlider.maxValue = GameManager.Instance.WinTimer;
        NumOfLives = GameManager.Instance.MonsterNumOfLives;
        livesText.text = "Lives: " + NumOfLives;
    }
	
	// Update is called once per frame
	void Update () {
        //Reset
        if (VisibilitySlider.value == VisibilitySlider.maxValue)
        {
            GameManager.Instance.Reset();
            GameManager.Instance.MonsterNumOfLives -= 1;
            VisibilitySlider.value = 0.0f;
        }
    }

    public void InitUI(int playerN)
    {
        playerNumber = playerN;

        //Set display to the correct player number
        GetComponent<Canvas>().targetDisplay = playerNumber;

        //Add to UIManager List
        UIManager.Instance.monsterUIsDictionary.Add(playerNumber, this);
    }

    //Turn on/off interaction text when hitting terminal
    public void ToggleMonsterInteractText(bool b)
    {
        interactText.enabled = b;
    }

    //Call to either see or hide the icon
    public void MonsterSeenUI(MonsterController monster, bool b)
    {
        //If true then add to slider
        if (b)
        {
            VisibilitySlider.value += Time.deltaTime;
            monster.monsterColor.a += monster.materialAlphaFadeRate * Time.deltaTime;
            monster.monsterMaterial.color = monster.monsterColor;
        }
        SeenImage.enabled = b;
    }

    //Call to drain the UI
    public void MonsterDrainUI(MonsterController monster)
    {
        VisibilitySlider.value -= Time.deltaTime;
        if (monster.monsterColor.a > 0.0f)
        {
            monster.monsterColor.a -= monster.materialAlphaFadeRate * Time.deltaTime;
            monster.monsterMaterial.color = monster.monsterColor;
        }
    }

    //To stop draining
    public IEnumerator stopDraining(MonsterController monster)
    {
        //Keep color semi - transparent
        drainColor.a = 0.35f;
        drainIcon.color = drainColor;

        yield return new WaitForSeconds(monster.drainLength);

        //Keep transparent when on cooldown
        drainColor.a = 0.35f;
        drainIcon.color = drainColor;

        monster.drainCooldown = true;

        yield return new WaitForSeconds(monster.drainCooldownLength);

        //Turn off
        drainColor.a = 1.0f;
        drainIcon.color = drainColor;

        monster.isDraining = false;
        monster.drainCooldown = false;
    }

    //To stop punching
    public IEnumerator stopPunching(MonsterController monster)
    {
        //Keep color semi - transparent
        punchColor.a = 0.35f;
        punchIcon.color = punchColor;

        yield return new WaitForSeconds(monster.punchLength);

        //Keep transparent when on cooldown
        punchColor.a = 0.35f;
        punchIcon.color = punchColor;

        monster.punchCooldown = true;

        yield return new WaitForSeconds(monster.punchCooldownLength);

        //Turn off
        punchColor.a = 1.0f;
        punchIcon.color = punchColor;

        monster.isPunching = false;
        monster.punchCooldown = false;
    }
}
