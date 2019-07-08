using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static InGameItem myItem;
    public static void SetInGameItem(InGameItem item)
    {
        myItem = item;
    }

    public static InGameItem GetInGameItem()
    {
        return myItem;
    }
}
