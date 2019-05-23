using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StartGame : MonoBehaviour {
    [SerializeField]
    private string levelName;
    public void StartTheGame()
    {
	SceneManager.LoadScene(levelName);
	MapManager.ResetValue();
	PoolManager.ResetValue();
    }

    public void ResetValue(){
        PlayerPrefs.SetInt("Coins", 0);
        string[] scenes = new string[SceneManager.sceneCountInBuildSettings];
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++){
	    scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
	for(int j = 1; j <= 3; j++){
            string sName = "Gem-" + scenes[i] + j;
            PlayerPrefs.SetInt(sName, 0);
        }
	    
    }
}
