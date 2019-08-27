using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {

    internal Pickup hiddenItem;
    internal int gemIndex;
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
                if (hiddenItem)
                {
                    Tile t = MapManager.Spawn(hiddenItem.name);
                    t.transform.position = transform.position;
                    t.GetComponent<Pickup>().time = 1.5f;
                    
                    if (t.GetComponent<Gem>())
                    {
                        Gem gem = t.GetComponent<Gem>();
                        gem.index = gemIndex;
                        gem.EvaluateGem(gemIndex);
                    }
                    t.GetComponent<Pickup>().DoEffect(player);
                }
                MapManager.Despawn(this.GetComponentInParent<Tile>());
            }
        }
    }
}
