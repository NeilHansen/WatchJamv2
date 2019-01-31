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

    public void SetDrainIcon(float dc = 0.35f)
    {
        //Keep color semi - transparent
        drainColor.a = dc;
        drainIcon.color = drainColor;
    }

    public void SetPunchIcon(float pc = 0.35f)
    {
        //Keep color semi - transparent
        punchColor.a = pc;
        punchIcon.color = punchColor;
    }
}
