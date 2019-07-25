using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/CreateItemInShop", order = 1)]
public class ItemData : ScriptableObject
{
    public Sprite itemImg;
    public float price;
    public int numOfUses = 1;
    public InGameItem myItem;

}
