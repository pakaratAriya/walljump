using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoveringButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    Character ch;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ch.notPlay = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ch.notPlay = false;
    }
    
    void Start()
    {
        ch = FindObjectOfType<Character>();
    }
}
