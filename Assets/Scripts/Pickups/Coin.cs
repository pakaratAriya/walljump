using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup {

    public override void DoEffect(Character player)
    {
        if (!player.dead)
        {
            SoundHelper.PlayCoinSfx();
            player.coin++;
            ParticleSystem pickupPar = PoolManager.Spawn("CoinParticle");
            pickupPar.transform.position = transform.position;
            StartCoroutine(DestroyPickUpObject());
        }
        
    }


}
