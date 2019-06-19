using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public GameObject completeLevelUI;
    public GameObject failureLevelUI;
    public static LevelManager levelManager;
    public Text coinNumTxt;
    public Image[] gemImages = new Image[3];
    public Sprite gemSprite;
    private static string levelName;
    public static bool STARTGAME = false;
    public static int LV_INDEX = 0;
    public static List<string> ALL_LEVEL;
    public SoundHelper soundHelperPref;
    
    private void Start()
    {
        if (SoundHelper.sh == null)
        {
            SoundHelper sh = Instantiate<SoundHelper>(soundHelperPref);
        }
        levelManager = this;
        levelName = SceneManager.GetActiveScene().name;
    }

    public static void CompleteLevel(int coin)
    {
        levelManager.completeLevelUI.SetActive(true);
        if (levelManager.coinNumTxt!=null)
        {
            levelManager.coinNumTxt.text = coin + "";
        }
        
        if(levelManager.gemSprite && levelManager.gemImages.Length == 3)
        {
            for (int i = 0; i < 3; i++)
                if(PlayerPrefs.GetInt("Gem-" + levelName + (i + 1)) == 1)
                    levelManager.gemImages[i].sprite = levelManager.gemSprite;
        }

    }

    public static void FailLevel()
    {
        levelManager.failureLevelUI.SetActive(true);
    }

    public void NextLevel()
    {
        SoundHelper.PlayClickSfx();
        if (ALL_LEVEL.Count > LV_INDEX)
        {
            SceneManager.LoadScene(ALL_LEVEL[LV_INDEX++]);
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void RetryLevel()
    {
        SoundHelper.PlayClickSfx();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelSelect()
    {
        SoundHelper.PlayClickSfx();
        SceneManager.LoadScene("LevelSelect");
    }

    public void MainMenu()
    {
        SoundHelper.PlayClickSfx();
        SceneManager.LoadScene("MenuScene");
    }
}
