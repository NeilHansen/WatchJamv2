﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterUI : MonoBehaviour {
    public static MonsterUI Instance;

    public Text interactText;

    public GameObject SeenImage;
    public Image VisibilitySlider;
    public Text livesText;

    public Image drainIcon;
    public Color drainColor;

    public Image punchIcon;
    public Color punchColor;

    public GameObject mountIcon;
    public GameObject dismountIcon;

    public TMP_Text GameTimerText;

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

        VisibilitySlider.fillAmount = 0;
        VisibilitySlider.fillAmount = 1;

        NumOfLives = GameManager.Instance.MonsterNumOfLives;
        livesText.text = "Lives: " + NumOfLives;
    }

    void Update()
    {
        SetGameTimerText(GameManager.Instance.GameTimer);
        livesText.text = "Lives: " + GameManager.Instance.MonsterNumOfLives;
    }

    public void SetGameTimerText(float time)
    {
        GameTimerText.text = "Timer: " + time;
    }

    public void ResetMonsterUI()
    {
        VisibilitySlider.fillAmount = 0.0f;
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
        SeenImage.SetActive(b);
    }

    public void SetVisibilitySlider(float value)
    {
        VisibilitySlider.fillAmount = value;
    }

    public void SetMountIcon(bool b)
    {
        //Keep color semi - transparent
        mountIcon.SetActive(b);
    }

    public void SetDismountIcon(bool b)
    {
        //Keep color semi - transparent
        dismountIcon.SetActive(b);
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
