using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItemSlot : MonoBehaviour
{
    BreakableObject bko;
    public HIDDEN_ITEM hiddenItem;
    public int gemIndex = 0;
    private void Start()
    {
        bko = GetComponentInChildren<BreakableObject>();
        if (hiddenItem != HIDDEN_ITEM.None)
        {
            Pickup pickup = Resources.Load<Pickup>("Items/" + hiddenItem);
            bko.hiddenItem = pickup;
        }
        bko.gemIndex = gemIndex;
    }
}

public enum HIDDEN_ITEM
{
    None,
    Coin,
    Gem
}



