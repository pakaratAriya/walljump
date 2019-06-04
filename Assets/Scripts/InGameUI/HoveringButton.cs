using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoveringButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    Character ch;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ch.hoveringButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ch.hoveringButton = false;
    }
    
    void Start()
    {
        ch = FindObjectOfType<Character>();
    }
}
