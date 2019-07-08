using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTriggerUI : MonoBehaviour
{
    public InGameItem item;
    public bool isUsed = false;
    public int numOfUse = 1;
    Character player; 

    private void Start()
    {
        player = FindObjectOfType<Character>();
        numOfUse = item.item.numOfUses;
        Image img = item.GetComponent<Image>();
        if (img)
        {
            img.sprite = item.item.itemImg;
        }
        item = ItemManager.GetInGameItem();
    }

    public void OnClickTrigger()
    {
        if (item == null)
        {
            return;
        }
        if (numOfUse > 0 && LevelManager.STARTGAME && player.closeWall && !player.dead && !player.charging)
        {
            Image img = item.GetComponent<Image>();
            numOfUse--;
            if (img && item.item.numOfUses == 0)
            {
                img.sprite = null;
            }
            item.TriggerItem();
            
            isUsed = true;
        }
    }
    
}
