using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour {
    public static MonsterUI Instance;

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
    void Awake()
    {
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start () {
        drainColor = drainIcon.color;
        punchColor = punchIcon.color;

        VisibilitySlider.value = 0;
        VisibilitySlider.maxValue = 1;

        NumOfLives = GameManager.Instance.MonsterNumOfLives;
        livesText.text = "Lives: " + NumOfLives;
    }
	
	// Update is called once per frame
	void Update () {
        //Reset
        if (VisibilitySlider.value == VisibilitySlider.maxValue)
        {
            VisibilitySlider.value = 0.0f;
            livesText.text = "Lives: " + NumOfLives;

            GameManager.Instance.MonsterNumOfLives -= 1;
            GameManager.Instance.Reset();
        }
    }

    //Turn on/off interaction text when hitting terminal
    public void ToggleMonsterInteractText(bool b)
    {
        interactText.enabled = b;
    }

    //Call to either see or hide the icon
    public void SetMonsterSeenIcon(bool b)
    {
        //Turn off seen imageg when alpha is 0
        SeenImage.enabled = b;
    }

    public void SetVisibilitySlider(float value)
    {
        VisibilitySlider.value = value;
    }

    public void StopDraining(MonsterController monster)
    {
        StartCoroutine(stopDraining(monster));
    }

    public void StopPunching(MonsterController monster)
    {
        StartCoroutine(stopPunching(monster));
    }

    //To stop draining
    private IEnumerator stopDraining(MonsterController monster)
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
    private IEnumerator stopPunching(MonsterController monster)
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
