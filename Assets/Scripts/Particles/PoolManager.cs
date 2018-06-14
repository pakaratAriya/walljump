using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private static Dictionary<string, ParticleSystem> particleBook = new Dictionary<string, ParticleSystem>();
    private static Dictionary<string, Stack<ParticleSystem>> particlePool = new Dictionary<string, Stack<ParticleSystem>>();
    List<ParticleSystem> parList = new List<ParticleSystem>();
    private static PoolManager poolManager;
    // Use this for initialization
    void Start () {
        poolManager = this;
        parList.AddRange(Resources.LoadAll<ParticleSystem>("Particles"));
        foreach (ParticleSystem par in parList)
        {
            if (!particleBook.ContainsKey(par.name))
            {
                particleBook.Add(par.name, par);
                particlePool[par.name] = new Stack<ParticleSystem>();
            }
            
        }
    }

    public static void ResetValue()
    {
        particleBook.Clear();
        particlePool.Clear();
    }

    public static ParticleSystem Spawn(string name)
    {
        ParticleSystem par = null;
        if (particlePool[name].Count <= 1)
        {
            par = Instantiate<ParticleSystem>(particleBook[name]);
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

    public static void Despawn(ParticleSystem par)
    {
        par.transform.parent = poolManager.transform;
        particlePool[par.name].Push(par);
        par.transform.localScale = Vector3.one;
        par.gameObject.SetActive(false);
    }
}
