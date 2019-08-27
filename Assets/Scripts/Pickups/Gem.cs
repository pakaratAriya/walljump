using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gem : Pickup
{
    internal string gemName;
    private InGameGemUI gemUi;
    
    [Range(1,3)]
    public int index = 0;

    private void Awake()
    {
        if (index != 0)
        {
            EvaluateGem(index);
        }
        
    }

    public void EvaluateGem(int index)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        gemUi = FindObjectOfType<InGameGemUI>();
        gemName = "Gem-" + sceneName + index;
        if (PlayerPrefs.GetInt(gemName) != 0)
        {
            gemUi.AddGem(index);
            Destroy(gameObject);
        }
    }

    public override void DoEffect(Character player)
    {
        if (!player.dead)
        {
            SoundHelper.PlayGemSfx();
            player.gem++;
            ParticleSystem pickupPar = PoolManager.Spawn("CoinParticle");
            pickupPar.transform.position = transform.position;
            StartCoroutine(DestroyPickUpObject());
            FindObjectOfType<Goal>().AddGem(gemName);
            gemUi.AddGem(index);
        }

    }
}
