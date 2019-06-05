using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelName : MonoBehaviour
{
    private string name;
    public Text levelTxt;
    private int levelIndex;
    [SerializeField]
    private Image[] gemUIs = new Image[3];
    public Sprite emptySpr;
    public Sprite gemSpr;

    public void SetGem()
    {
        for (int i = 1; i <= 3; i++)
        {
            string gemName = "Gem-" + name + i;
            if (PlayerPrefs.GetInt(gemName) != 0)
            {
                gemUIs[i - 1].sprite = gemSpr;
            }
        }
        
    }

    public void SetLevelName(string n, int i)
    {
        name = n;
        levelIndex = i + 1;
        levelTxt.text = "" + levelIndex;
        SetGem();
    }
    public string GetLevelName()
    {
        return name;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(name);
    }
}
