using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItem : MonoBehaviour
{
    protected Character myChar;
    private void Awake()
    {
        myChar = FindObjectOfType<Character>();
    }

    public virtual void TriggerItem()
    {

    }
}
