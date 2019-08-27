using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    protected bool isPicked = false;
    internal float time = 1;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Character>() != null && !isPicked)
        {
            DoEffect(col.GetComponent<Character>());
            isPicked = true;
        }
    }

    public virtual void DoEffect(Character player)
    {

    }

    public void ResetState()
    {
        anim.SetBool("IsPicked", false);
        Color c = sr.color;
        c.a = 1;
        sr.color = c;
        isPicked = false;
    }

    protected IEnumerator DestroyPickUpObject()
    {
        if (anim)
        {
            isPicked = true;
            if (rb)
            {
                rb.velocity = Vector2.up * 0.5f;
            }
            anim.SetBool("IsPicked", true);
            //yield return new WaitForSeconds(1.5f);
            if (sr)
            {
                while(sr.color.a > 0)
                {
                    Color myColor = sr.color;
                    myColor.a -= Time.deltaTime/time;
                    sr.color = myColor;
                    yield return new WaitForEndOfFrame();
                }
            }
            MapManager.Despawn(GetComponent<Tile>());
        }
        else
        {
            MapManager.Despawn(GetComponent<Tile>());
        }
        
    }
}
