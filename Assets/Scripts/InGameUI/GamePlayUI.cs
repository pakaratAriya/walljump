﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    public GameObject PauseMenu;
    private Character ch;

    void Start()
    {
        ch = FindObjectOfType<Character>();
    }

    public void PauseGame()
    {
        SoundHelper.PlayClickSfx();
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        ch.notPlay = true;
    }

    public void ResumeGame()
    {
        SoundHelper.PlayClickSfx();
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        ch.notPlay = false;
    }

}
