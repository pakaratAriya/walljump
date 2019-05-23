using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gem : Pickup
{
    public string gemName;
    
    [Range(1,3)]
    public int index = 1;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        gemName = "Gem-" + sceneName + index;
        if (PlayerPrefs.GetInt(gemName)!=0){
            Destroy(gameObject);
        }
    }

    protected override void DoEffect(Character player)
    {
        if (!player.dead)
        {
            player.gem++;
            ParticleSystem pickupPar = PoolManager.Spawn("CoinParticle");
            pickupPar.transform.position = transform.position;
            MapManager.Despawn(GetComponent<Tile>());
            FindObjectOfType<Goal>().AddGem(gemName);
        }

    }
}
