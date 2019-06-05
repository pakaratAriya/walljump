using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public List<string> levels = new List<string>();
    public LevelName levelUiPref;
    private Canvas canvas;
    

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        for(int i = 0; i < levels.Count; i++)
        {
            LevelName lvl = Instantiate<LevelName>(levelUiPref);
            lvl.SetLevelName(levels[i], i);
            Image img = lvl.GetComponent<Image>();
            img.transform.parent = canvas.transform;
            img.rectTransform.localPosition = new Vector2(-140 + (70 * (i % 5)), 160 - (80 * (i / 5)));
            
        }
    }
}
