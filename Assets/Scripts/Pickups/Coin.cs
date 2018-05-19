using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup {

    protected override void DoEffect(Character player)
    {
        ParticleSystem pickupPar = PoolManager.Spawn("CoinParticle");
        pickupPar.transform.position = transform.position;
        MapManager.Despawn(GetComponent<Tile>());
    }
}
