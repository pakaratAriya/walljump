using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public ItemInShop ItemPref;
    public List<ItemData> itemInfo;
    internal Canvas canvas;
    public Image selectedItemImg;



    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        for (int i = 0; i < itemInfo.Count; i++)
        {
            ItemInShop itemIs = Instantiate<ItemInShop>(ItemPref);
            itemIs.selectedImg = selectedItemImg;
            itemIs.SetItem(itemInfo[i]);
            Image img = itemIs.GetComponent<Image>();
            img.transform.parent = canvas.transform;
            img.rectTransform.localPosition = new Vector2(-140 + (70 * (i % 5)), 160 - (80 * (i / 5)));
            itemIs.name = ItemPref.name;
        }
    }

    public void GetItem()
    {

    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MenuScene");
    }



}
