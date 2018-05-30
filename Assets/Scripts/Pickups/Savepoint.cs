using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : MonoBehaviour {

    Vector3 parentPos;
    Animator anim;
    internal bool saved;
    public int saveNum = 1;

    private void Start()
    {
        parentPos = transform.parent.position;
        anim = GetComponent<Animator>();
        if (saved == true)
        {
            anim.SetBool("isSaved", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Character>() != null && saved == false)
        {
            MapManager.SetPlayerSavePoint(this, parentPos.x, parentPos.y, transform.parent.name);
            anim.SetBool("isSaved", true);
            ParticleSystem light = PoolManager.Spawn("SavingLight");
            light.transform.position = transform.position;
            StartCoroutine(CreateExplode());
            saved = true;
        }
    }

    private IEnumerator CreateExplode()
    {
        for (int i = 0; i <= 3; i++)
        {
            ParticleSystem burst = PoolManager.Spawn("BurstParticle");
            Vector3 pos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            burst.transform.position = transform.position + pos;
            yield return new WaitForSeconds(0.2f);
        }
        
    }

}
