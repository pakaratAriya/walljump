using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gem : Pickup
{
    public string gemName;
    private InGameGemUI gemUi;
    
    [Range(1,3)]
    public int index = 1;

    private void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        gemUi = FindObjectOfType<InGameGemUI>();
        gemName = "Gem-" + sceneName + index;
        if (PlayerPrefs.GetInt(gemName)!=0){
            gemUi.AddGem(index);
            Destroy(gameObject);
        }
    }

    protected override void DoEffect(Character player)
    {
        if (!player.dead)
        {
            SoundHelper.PlayGemSfx();
            player.gem++;
            ParticleSystem pickupPar = PoolManager.Spawn("CoinParticle");
            pickupPar.transform.position = transform.position;
            MapManager.Despawn(GetComponent<Tile>());
            FindObjectOfType<Goal>().AddGem(gemName);
            gemUi.AddGem(index);
        }

    }
}
