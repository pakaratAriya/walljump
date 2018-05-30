using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    ParticleSystem par;
	// Use this for initialization
	void Awake () {
        par = GetComponent<ParticleSystem>();
	}

    private void OnEnable()
    {
        par.Play();
    }
    // Update is called once per frame
    void Update () {
        if (!par.isPlaying)
        {
            PoolManager.Despawn(par);
        }
	}
}
