using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTriggerUI : MonoBehaviour
{
    public InGameItem item;
    public bool isUsed = false;

    private void Start()
    {
        Image img = item.GetComponent<Image>();
        if (img)
        {
            img.sprite = item.item.itemImg;
        }
    }

    public void OnClickTrigger()
    {
        if (item.item.numOfUses > 0 && LevelManager.STARTGAME)
        {
            Image img = item.GetComponent<Image>();
            item.item.numOfUses--;
            if (img && item.item.numOfUses == 0)
            {
                img.sprite = null;
            }
            item.TriggerItem();
            
            isUsed = true;
        }
    }
    
}
