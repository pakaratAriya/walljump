using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy {

    private float speed = 1;
    public state snailState = state.Idle;
    private float initLength = 15;
    private float facingDirection = -1;


    protected override void Awake()
    {
        base.Awake();
        leftScale = 1;
        rightScale = -1;
        leftAngle = -90;
        rightAngle = 90;
        leftOffset = 0.8f;
        rightOffset = -0.8f;
    }

    private void OnEnable()
    {
        snailState = state.Idle;
        deathZone.enable = true;
        rb.freezeRotation = true;
        rb.gravityScale = 0;
        originalScale = transform.localScale.x;
        direction = new Vector3(originalScale, 0, 0);

    }

    protected override void Update()
    {
        base.Update();
        if (snailState == state.Walk)
        {
            transform.Translate(direction * Time.deltaTime * speed);
        }
        else
        {
            if (MapManager.player.transform.position.y + initLength >= transform.position.y)
            {
                snailState = state.Walk;
            }
        }
        RaycastHit2D hits;
        RaycastHit2D hitsDown;
        hits = Physics2D.Raycast(transform.position + Vector3.left * originalScale * 0.4f + Vector3.up * facingDirection * 0.55f
            , Vector3.up * facingDirection, 0.2f);
        hitsDown = Physics2D.Raycast(transform.position + Vector3.left * originalScale * 0.4f + Vector3.up * facingDirection * 0.55f
            , Vector3.up * facingDirection + Vector3.left * originalScale, 0.2f);
        if (hits.collider != null)
        {
            if ((hits.collider.GetComponent<DeathZone>() != null && hits.collider.GetComponent<DeathZone>() != deathZone)
                || hits.collider.GetComponent<TrapTrigger>() != null || hits.collider.GetComponent<Tile>() != null)
            {
                TurnBack();
            }
        }
        if (hitsDown.collider == null)
        {
            TurnBack();
        }

    }

    public void TurnBack()
    {
        direction *= -1;
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public override void Die()
    {
        if(snailState != state.Idle)
        {
            deathZone.enable = false;
            rb.freezeRotation = false;
            rb.AddTorque(100);
            rb.gravityScale = 1;
        }
        
    }

}

public enum state
{
    Idle,
    Walk
};
