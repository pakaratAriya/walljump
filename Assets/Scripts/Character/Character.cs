using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : Unit {

    internal bool closeWall = true;
    public int direction = 1;
    internal Rigidbody2D rb;
    public float minPow = 8;
    public float maxPow = 15;
    public float power = 0;
    public float chargeRate = 0.3f;
    public bool charging = false;
    public bool sliding = false;
    public bool dashing = false;
    public bool dead = false;
    public bool invulnerable = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            return;
        }
		if (Input.GetKeyDown(KeyCode.Space))
        {
            if (closeWall)
            {
                charging = true;
            }
        }
        if (charging)
        {
            StartCharging();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (rb.gravityScale != 0)
            {
                Dash();
            }
            else if (charging)
            {
                charging = false;
                power = 0;
            } else
            {
                sliding = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            sliding = false;
        }
            if (sliding)
        {
            Slide();
        }
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(dead)
        {
            return;
        }
        if (Mathf.Abs(col.transform.position.x) - Mathf.Abs(transform.position.x) <= 0.3f)
        {
            return;
        }
        closeWall = true;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        transform.localScale = new Vector3(direction, 1, 1);
        dashing = false;
    }

    /// <summary>
    /// the player makes a jump
    /// </summary>
    public void Jump()
    {
        if (closeWall && charging)
        {
            power = (power >= minPow) ? power : minPow;
            closeWall = false;
            rb.velocity = new Vector3(7 * direction, power, 0);
            direction *= -1;
            power = 0;
            rb.gravityScale = 1.5f;
            ParticleSystem par = PoolManager.Spawn("dustParticle");
            par.transform.position = transform.position + Vector3.up*1 + Vector3.left * transform.localScale.x * 0.2f;
            charging = false;
        }
    }

    /// <summary>
    /// call from deathzone class to bounce the character back to another side.
    /// </summary>
    internal void EnemyJump()
    {
        transform.localScale = new Vector3(direction, 1, 1);
        rb.velocity = new Vector3(7 * direction, minPow, 0);
        direction *= -1;
        power = 0;
        rb.gravityScale = 1.5f;
    }

    private void Slide()
    {
        if (CanSlide())
        {
            transform.Translate(0, -3 * Time.deltaTime, 0);
            ParticleSystem par = PoolManager.Spawn("dustParticle");
            par.transform.position = transform.position + Vector3.up * 0.5f + Vector3.left * transform.localScale.x * 0.2f;
            sliding = true;
        }
        
        
    }

    private bool CanSlide()
    {
        RaycastHit2D hits;
        RaycastHit2D hitsDown;
        hits = Physics2D.Raycast(transform.position + Vector3.down * 0.4f, Vector3.down, 0.3f, ~LayerMask.GetMask("player"));
        hitsDown = Physics2D.Raycast(transform.position + Vector3.down * 0.4f, Vector3.down + Vector3.left * transform.localScale.x, 0.7f, ~LayerMask.GetMask("player"));
        
        if (hits.collider != null) 
        {
            print(hits.collider.name);
            return (hits.collider.GetComponent<Tile>() == null);
        }
        return hitsDown.collider != null;
    }

    /// <summary>
    /// player start charging.
    /// </summary>
    public void StartCharging()
    {
        if (GetCloseWall())
        {
            charging = true;
            power = (power >= maxPow) ? maxPow : power + chargeRate;
        } 
    }

    public void Dash()
    {
        rb.velocity = new Vector3(20 * transform.localScale.x, 0, 0);
        rb.gravityScale = 0.2f;
        dashing = true;
    }

    public bool GetCloseWall()
    {
        return closeWall;
    }

    public override void Die()
    {
        if (!dead && !invulnerable)
        {
            dead = true;
            GetComponent<TrailRenderer>().enabled = false;
            rb.freezeRotation = false;
            rb.AddTorque(100);
            rb.gravityScale = 2;
            StartCoroutine(RestartGame());
        }
        
    }

    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
