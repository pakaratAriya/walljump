using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInShop : MonoBehaviour
{
    public Image img;
    public Text priceTxt;
    internal Image selectedImg;
    private ItemData myItem;

    public void SetItem(ItemData iData)
    {
        myItem = iData;
        img.sprite = iData.itemImg;
        if (PlayerPrefs.GetString("currentItem").Equals(iData.name))
        {
            selectedImg.sprite = myItem.itemImg;
        }

        priceTxt.text = iData.price + "";
    }

    public void SelectItem()
    {
        selectedImg.sprite = myItem.itemImg;
        ItemManager.SetInGameItem(myItem.myItem);
        PlayerPrefs.SetString("currentItem", myItem.name);
    }
}
