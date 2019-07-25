using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    public List<string> levels = new List<string>();
    public LevelName levelUiPref;
    private Canvas canvas;
    

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        for(int i = 0; i < levels.Count; i++)
        {
            LevelName lvl = Instantiate<LevelName>(levelUiPref);
            string nextLevel = (i + 1) < levels.Count ? levels[i + 1] : "MenuScene";
            lvl.SetLevelName(levels[i], i, nextLevel);
            Image img = lvl.GetComponent<Image>();
            img.transform.parent = canvas.transform;
            img.rectTransform.localPosition = new Vector2(-140 + (70 * (i % 5)), 160 - (80 * (i / 5)));
            LevelManager.ALL_LEVEL = levels;
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MenuScene");

    }
}
