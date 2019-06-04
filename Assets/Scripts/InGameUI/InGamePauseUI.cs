using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePauseUI : MonoBehaviour
{
    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

}
