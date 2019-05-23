using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    [SerializeField]
    private string levelName;
    public void StartTheGame()
    {
	SceneManager.LoadScene(levelName);
	MapManager.ResetValue();
	PoolManager.ResetValue();
    }
}
