using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableObject : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Character>() != null)
        {
            Character player = col.GetComponent<Character>();
            if (player.dashing)
            {
                ParticleSystem par1 = PoolManager.Spawn("breakableParticle");
                par1.transform.position = transform.position;
                ParticleSystem par2 = PoolManager.Spawn("dust2Particle");
                par2.transform.position = transform.position;
                MapManager.Despawn(this.GetComponentInParent<Tile>());
            }
        }
    }
}
