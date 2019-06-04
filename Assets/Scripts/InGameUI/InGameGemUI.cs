using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameGemUI : MonoBehaviour
{
    [SerializeField]
    private Image[] gemUIs = new Image[3];
    public Sprite emptySpr;
    public Sprite gemSpr;

    public void AddGem(int index)
    {
        gemUIs[index - 1].sprite = gemSpr;
    }
    public void DeleteGem(int index)
    {
        gemUIs[index - 1].sprite = emptySpr;
    }



}
