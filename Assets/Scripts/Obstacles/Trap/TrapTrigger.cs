using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour {
    private Animator anim;
    public DeathZone deathZone;
    private Vector3 firstScale;
    private Vector3 firstPos;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        deathZone = GetComponentInChildren<DeathZone>();
        firstPos = transform.localPosition;
    }

    private void OnEnable()
    {
        if (anim != null)
        {
            anim.Rebind();
        }
        deathZone.enable = false;
        deathZone.GetComponent<Rigidbody2D>().simulated = false;
        deathZone.transform.localPosition = Vector3.zero;
        deathZone.transform.localRotation = Quaternion.identity;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>() != null)
        {
            if (anim != null)
            {
                anim.SetTrigger("TrapTrigger");
                StartCoroutine(Explode());
            }
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("StartExplode");
        yield return new WaitForSeconds(0.5f);
        deathZone.enable = true;
        deathZone.GetComponent<Rigidbody2D>().simulated = true;
        deathZone.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        deathZone.GetComponent<Rigidbody2D>().AddForce(deathZone.transform.localScale * 10);
        deathZone.GetComponent<Rigidbody2D>().AddTorque(1000);
        yield return new WaitForSeconds(0.3f);
        deathZone.enable = false;
        
        
    }
}
