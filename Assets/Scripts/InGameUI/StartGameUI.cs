using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : MonoBehaviour
{
    [SerializeField]
    private Text text;
    float timer = 3;

    void Update()
    {
        
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            text.text = "" + Mathf.CeilToInt(timer);
        }
        else if(timer >= -1)
        {
            text.text = "START";
        }
        else
        {
            LevelManager.STARTGAME = true;
            gameObject.SetActive(false);
        }
    }
}
