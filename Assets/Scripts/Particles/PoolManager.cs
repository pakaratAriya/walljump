using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private static Dictionary<string, WallJumpParticle> particleBook = new Dictionary<string, WallJumpParticle>();
    private static Dictionary<string, Stack<WallJumpParticle>> particlePool = new Dictionary<string, Stack<WallJumpParticle>>();
    List<WallJumpParticle> parList = new List<WallJumpParticle>();
    private static PoolManager poolManager;
    // Use this for initialization
    void Start () {
        poolManager = this;
        parList.AddRange(Resources.LoadAll<WallJumpParticle>("Particles"));
        foreach (WallJumpParticle par in parList)
        {
            particleBook.Add(par.name, par);
            particlePool[par.name] = new Stack<WallJumpParticle>();
        }
    }
    public static WallJumpParticle Spawn(string name)
    {
        WallJumpParticle par = null;
        if (particlePool[name].Count <= 1)
        {
            par = Instantiate<WallJumpParticle>(particleBook[name]);
            par.name = name;
        }
        else
        {
            par = particlePool[name].Pop();
            par.gameObject.SetActive(true);
        }
        par.transform.SetParent(poolManager.transform);
        return par;
    }

    public static void Despawn(WallJumpParticle par)
    {
        particlePool[par.name].Push(par);
        par.gameObject.SetActive(false);
    }
}
