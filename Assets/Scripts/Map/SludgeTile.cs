using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SludgeTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.GetComponent<Character>())
        {
            col.collider.GetComponent<Character>().falling = true;
        }
    }

    
}
