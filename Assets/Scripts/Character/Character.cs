using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : Unit {

    public bool closeWall = true;
    public int direction = 1;
    public Rigidbody2D rb;
    public float minPow = 8;
    public float maxPow = 15;
    public float power = 0;
    public float chargeRate = 0.6f;
    public bool charging = false;
    public bool sliding = false;
    public bool dashing = false;
    public bool dead = false;
    public bool invulnerable = false;
    public bool climbing = false;
    internal Animator anim;
    public bool onStand = false;
    public bool chargeUp = true;
    public bool debugLine = false;
    public CharacterHelper ch;
    public int coin = 0;
    public int gem = 0;
    internal bool notPlay = false;
    SpriteRenderer sr;
    public static bool STARTGAME = false;
	// Use this for initialization
	void Awake () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ch = GetComponentInChildren<CharacterHelper>();
        if (ch)
            ch.line.enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
       
        if (dead || notPlay || !STARTGAME)
        {
            return;
        }
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (closeWall && !climbing)
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
            DebugLineSize();
            StartCharging();
        }
        
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            
            Jump();
            chargeUp = true;
            //DisableLineSize();
        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
               
            if (rb.gravityScale != 0)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
                anim.SetBool("Dash", true);
                /*for(int i = 0; i < 10; i++)
                {
                    //ParticleSystem par = PoolManager.Spawn("ChargeParticle");
                    //par.transform.position = transform.position;
                }
                */
                Invoke("Dash", 0.2f);
            }
            //else if (charging && !onStand)
            //{
            //    charging = false;
            //    power = 0;
            //    anim.SetBool("Charging", false);
            //}
            //else if (onStand)
            //{
            //    direction = -1;
            //    transform.localScale = new Vector3(direction, 1, 1);
            //    charging = true;
            //}
            //else
            //{
            //    sliding = true;
            //}
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

    private void DebugLineSize()
    {
        if (ch && debugLine)
        {
            ch.line.enabled = true;
            float sendingPow;
            sendingPow = (power >= minPow) ? power : minPow;
            ch.DrawTraject(transform.position, new Vector2(7 * direction, sendingPow));

        }
    }

    private void DisableLineSize()
    {
        if(ch)
            ch.line.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
        {
            return;
        }

        if (col.collider.GetComponent<UngrabablePath>() != null)
        {
            if (dashing)
            {
                print(col.collider.transform.position.y - transform.position.y);
                if(col.collider.transform.position.y - transform.position.y <= 0.8f)
                {
                    anim.SetBool("Dash", false);
                    
                    rb.gravityScale = 1.5f;
                    rb.velocity = Vector3.zero;
                    dashing = false;
                }
            }
            return;
        }

        if(rb.velocity.y > 12)
        {
            return;
        }

        if (col.collider.GetComponent<StandablePath>() != null)
        {
            direction *= -1;
            onStand = true;
        }
        
        if (col.collider.GetComponent<ClimbableArea>() != null)
        {
            StartCoroutine(ClimbUp(col.collider.transform.position + Vector3.up));
        }

        
        anim.SetBool("Dash", false);
        anim.SetBool("TouchWall", true);
        closeWall = true;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        transform.localScale = new Vector3(direction, 1, 1);
        dashing = false;
    }

    IEnumerator ClimbUp(Vector3 des)
    {
        climbing = true;
        float movingSpeed = 0.05f;
        bool isLeft = des.x < transform.position.x;
        float shiftDis = isLeft ? -movingSpeed : movingSpeed;
        while(transform.position.y < des.y)
        {
            transform.position += Vector3.up * movingSpeed;
            yield return new WaitForEndOfFrame();
        }
        while((isLeft && transform.position.x > des.x) || (!isLeft && transform.position.x < des.x))
        {
            transform.position += Vector3.right * shiftDis;
            yield return new WaitForEndOfFrame();
        }
        climbing = false;

    }

    /// <summary>
    /// the player makes a jump
    /// </summary>
    public void Jump()
    {
        if (closeWall && charging && !climbing)
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

    public IEnumerator InvincibleEffect(float duration)
    {
        bool decreaseAlpha = true;
        float changeSpeed = 5 * Time.deltaTime;
        while(invulnerable)
        {
            if (decreaseAlpha)
            {
                sr.color = new Color(sr.color.r, sr.color.g - changeSpeed, sr.color.b - changeSpeed);
            }
            else
            {
                sr.color = new Color(sr.color.r , sr.color.g + changeSpeed, sr.color.b + changeSpeed);
            }
            if(sr.color.g <= 0)
            {
                decreaseAlpha = false;
            }
            if(sr.color.g > 0.6)
            {
                decreaseAlpha = true;
            }
            yield return new WaitForEndOfFrame();
        }
        sr.color = new Color(1, 1, 1);
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
        if (GetCloseWall() && !climbing)
        {
            //anim.SetBool("Charging", true);
            charging = true;
            if (chargeUp == true)
            {
                if(power< maxPow)
                {
                    power += chargeRate;
                }
                else
                {
                    power = maxPow;
                    chargeUp = false;
                }
            }
            else
            {
                if (power > 0)
                {
                    power -= chargeRate;
                }
                else
                {
                    power = 0;
                    chargeUp = true;
                }
            }
            //power = (power >= maxPow) ? maxPow : power + chargeRate;
            //ParticleSystem par = PoolManager.Spawn("ChargeParticle");
            //par.transform.position = transform.position - Vector3.up * 0.2f;
            //par.transform.parent = transform;
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

    public void ResetState()
    {
        charging = false;
    }
}
