using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public GameObject monsterInteractUI;

    public Image SeenUI;
    public Slider MonsterUI;
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
    void Start()
    {
        drainColor = drainIcon.color;
        punchColor = punchIcon.color;

        MonsterUI.maxValue = GameManager.Instance.WinTimer;
        NumOfLives = GameManager.Instance.MonsterNumOfLives;
        livesText.text = "Lives: " + NumOfLives;
    }

    // Update is called once per frame
    void Update()
    {
        MonsterUIUpdate();
        PlayerUIUpdate();
    }

    #region Player Stuff
    private void PlayerUIUpdate()
    {

    }
    #endregion

    #region Monster Stuff
    private void MonsterUIUpdate()
    {
        //Reset
        if (MonsterUI.value == MonsterUI.maxValue)
        {
            GameManager.Instance.Reset();
            GameManager.Instance.MonsterNumOfLives -= 1;
            MonsterUI.value = 0.0f;
        }
    }

    //Call to either see or hide the icon
    public void MonsterSeenUI(bool b)
    {
        //If true then add to slider
        if(b)
        {
            MonsterUI.value += Time.deltaTime;
            GameManager.Instance.monsterController.monsterColor.a += GameManager.Instance.monsterController.materialAlphaFadeRate * Time.deltaTime;
            GameManager.Instance.monsterController.monsterMaterial.color = GameManager.Instance.monsterController.monsterColor;
        }
        SeenUI.enabled = b;
    }

    //Call to drain the UI
    public void MonsterDrainUI()
    {
        MonsterUI.value -= Time.deltaTime;
        if(GameManager.Instance.monsterController.monsterColor.a > 0.0f)
        {
            GameManager.Instance.monsterController.monsterColor.a -= GameManager.Instance.monsterController.materialAlphaFadeRate * Time.deltaTime;
            GameManager.Instance.monsterController.monsterMaterial.color = GameManager.Instance.monsterController.monsterColor;
        }
    }
    #endregion
}
