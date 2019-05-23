using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoin : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Coin: " + PlayerPrefs.GetInt("Coins");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
