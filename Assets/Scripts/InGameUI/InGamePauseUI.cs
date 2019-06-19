using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePauseUI : MonoBehaviour
{
    public void RetryButton()
    {
        SoundHelper.PlayClickSfx();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void GoMainMenu()
    {
        SoundHelper.PlayClickSfx();
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

}
