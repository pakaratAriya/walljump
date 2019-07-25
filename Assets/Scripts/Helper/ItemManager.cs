using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static InGameItem myItem;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        string itemName = PlayerPrefs.GetString("currentItem");
        print(itemName);
        if (itemName!=null)
        {
            myItem = Resources.Load<ItemData>("ItemInShop/" + itemName).myItem;
        }
    }

    public static void SetInGameItem(InGameItem item)
    {
        myItem = item;
    }

    public static InGameItem GetInGameItem()
    {
        return myItem;
    }
}
