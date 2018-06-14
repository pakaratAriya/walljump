using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour {

    public bool enable = true;
    public bool dashable = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Unit>() != null && enable)
        {
            if (dashable && col.GetComponent<Character>() != null)
            {
                Character player = col.GetComponent<Character>();
                if (col.GetComponent<Character>().dashing)
                {
                    GetComponent<Unit>().Die();
                    
                    player.EnemyJump();
                } else
                {
                    player.Die();
                }
            } else
            {
                if (col.GetComponent<Character>())
                {
                    col.GetComponent<Character>().Die();
                }
                
            }
            
        }
    }
}
