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
    internal Animator anim;
    public bool onStand = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
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
                if (onStand)
                {
                    direction = 1;
                    transform.localScale = new Vector3(direction, 1, 1);
                }
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
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
                anim.SetBool("Dash", true);
                for(int i = 0; i < 10; i++)
                {
                    ParticleSystem par = PoolManager.Spawn("ChargeParticle");
                    par.transform.position = transform.position;
                }
                
                Invoke("Dash", 0.2f);

            }
            else if (charging && !onStand)
            {
                charging = false;
                power = 0;
                anim.SetBool("Charging", false);
            } else if (onStand)
            {
                direction = -1;
                transform.localScale = new Vector3(direction, 1, 1);
                charging = true;
            }
            else
            {
                sliding = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            sliding = false;
            if (onStand)
            {
                
                Jump();
                
            }
        }
            if (sliding)
        {
            anim.SetBool("Slide", true);
            Slide();
        } else
        {
            anim.SetBool("Slide", false);
        }
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
        {
            return;
        }

        if (col.collider.GetComponent<UngrabablePath>() != null)
        {
            return;
        }

        if(rb.velocity.y > 12)
        {
            return;
        }

        if (col.collider.GetComponent<StandablePath>() != null)
        {
            onStand = true;
        }



        
        anim.SetBool("Dash", false);
        anim.SetBool("TouchWall", true);
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
            onStand = false;
            GetComponent<TrailRenderer>().enabled = false;
            anim.SetBool("Charging", false);
            anim.SetTrigger("Jump");
            anim.SetBool("TouchWall", false);
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
        GetComponent<TrailRenderer>().enabled = false;
        StartCoroutine(TempInvulnurable());
        dashing = false;
        anim.SetTrigger("Jump");
        anim.SetBool("Dash", false);
        transform.localScale = new Vector3(direction, 1, 1);
        rb.velocity = new Vector3(7 * direction, minPow, 0);
        direction *= -1;
        power = 0;
        rb.gravityScale = 1.5f;
    }

    internal IEnumerator TempInvulnurable()
    {
        invulnerable = true;
        yield return new WaitForSeconds(0.2f);
        invulnerable = false;
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
            anim.SetBool("Charging", true);
            charging = true;
            power = (power >= maxPow) ? maxPow : power + chargeRate;
            ParticleSystem par = PoolManager.Spawn("ChargeParticle");
            par.transform.position = transform.position - Vector3.up * 0.2f;
            par.transform.parent = transform;
        } 
    }

    public void Dash()
    {
        
        rb.velocity = new Vector3(20 * transform.localScale.x, 0, 0);
        dashing = true;
        GetComponent<TrailRenderer>().enabled = true;
    }

    public bool GetCloseWall()
    {
        GetComponent<TrailRenderer>().enabled = false;
        return closeWall;
    }

    public override void Die()
    {
        if (!dead && !invulnerable)
        {
            anim.SetTrigger("Die");
            anim.speed = 0;
            Invoke("HeroDie", 0.3f);
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            dead = true;
        }
        
    }

    private void HeroDie()
    {
        anim.speed = 1;

        GetComponent<TrailRenderer>().enabled = false;
        rb.freezeRotation = false;
        rb.AddTorque(10 * transform.localScale.x);
        rb.velocity = new Vector3(transform.localScale.x * -3, 2, 0);
        rb.gravityScale = 2;
        StartCoroutine(RestartGame());
    }

    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);
        MapManager.PlayerDie();
    }
}
