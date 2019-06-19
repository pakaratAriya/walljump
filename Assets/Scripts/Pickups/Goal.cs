using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    // Update is called once per frame
    float spinSpeed = 30;
    private List<string> gemArray = new List<string>();
    Character ch;
    void Update () {
	transform.Rotate(0,0,Time.deltaTime * spinSpeed);
	if(ch){
            
            ch.transform.Rotate(0, 0, Time.deltaTime * spinSpeed * 10);
            ch.transform.localScale *= 0.98f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
	if(col.GetComponent<Character>()!=null){
            ch = col.GetComponent<Character>();
            ch.transform.parent = this.transform;
            ch.transform.position = transform.position;
            ch.GetComponent<Character>().rb.velocity = Vector3.zero;
            ch.GetComponent<Character>().rb.isKinematic = true;
            Invoke("EndLevel", 2);
        }
    }

    public void AddGem(string gem){
        gemArray.Add(gem);
    }

    void EndLevel(){
        //SceneManager.LoadScene("MenuScene");
        int c = PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", c + ch.coin);
	    foreach(string gem in gemArray){
            PlayerPrefs.SetInt(gem, 1);
        }
        LevelManager.CompleteLevel(ch.coin);
    }


}
