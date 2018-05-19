using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Character>() != null)
        {
            DoEffect(col.GetComponent<Character>());
            
        }
    }

    protected virtual void DoEffect(Character player)
    {

    }
}
