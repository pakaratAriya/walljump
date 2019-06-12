using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTriggerUI : MonoBehaviour
{
    public InGameItem item;
    public bool isUsed = false;

    public void OnClickTrigger()
    {
        if (!isUsed)
        {
            Image img = item.GetComponent<Image>();
            if (img)
            {
                img.sprite = null;
            }
            item.TriggerItem();
            
            isUsed = true;
        }
    }
    
}
