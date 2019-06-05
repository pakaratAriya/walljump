using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

    public DeathZone deathZone;
    protected Rigidbody2D rb;
    internal float leftScale;
    internal float rightScale;
    internal float leftAngle;
    internal float rightAngle;
    internal float leftOffset;
    internal float rightOffset;
    public float originalScale;
    internal Vector3 direction = new Vector3(-1, 0, 0);

    protected virtual void Awake()
    {
        deathZone = GetComponentInChildren<DeathZone>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Reset()
    {
        deathZone.enabled = true;
    }

    protected virtual void Update()
    {
        if (MapManager.player)
        {
            if (transform.position.y < MapManager.player.transform.position.y - 20)
            {
                MapManager.DespawnEnemy(this);
            }
        }
        
    }

}
