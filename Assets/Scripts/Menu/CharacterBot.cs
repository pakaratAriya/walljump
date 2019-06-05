using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBot : MonoBehaviour
{
    public Character ch;

    private void Awake()
    {
        ch = GetComponent<Character>();
        ch.notPlay = true;
        
    }

    private void OnDisable()
    {
        ch.rb.gravityScale = 0;
        ch.rb.velocity = Vector3.zero;
        ch.closeWall = true;
    }

    private void OnEnable()
    {
        StartCoroutine(JumpLoop());
    }


    IEnumerator JumpLoop()
    {
        while (true)
        {
            ch.power = 5;
            ch.Jump();
            ch.charging = true;
            yield return new WaitForSeconds(1);
        }
    }
}
