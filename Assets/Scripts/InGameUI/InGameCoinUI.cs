using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCoinUI : MonoBehaviour
{
    private Character ch;
    [SerializeField]
    private Text coinTxt;

    private void Start()
    {
        ch = FindObjectOfType<Character>();
    }

    private void Update()
    {
        if (coinTxt)
        {
            coinTxt.text = "" + ch.coin;
        }
    }
} 
