using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    bool closeWall = true;
    public int direction = 1;
    Rigidbody2D rb;
    public float minPow = 8;
    public float maxPow = 15;
    public float power = 0;
    public float chargeRate = 0.3f;
    public bool charging = false;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) && GetCloseWall())
        {
            charging = true;
        }
        if (charging)
        {
            StartCharging();
        }
        if (Input.GetKeyUp(KeyCode.Space) && charging)
        {
            Jump();
            charging = false;
        }
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        closeWall = true;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void Jump()
    {
        if (closeWall)
        {
            power = (power >= minPow) ? power : minPow;
            closeWall = false;
            rb.velocity = new Vector3(10 * direction, power, 0);
            direction *= -1;
            power = 0;
            rb.gravityScale = 1;
            WallJumpParticle par = PoolManager.Spawn("dustParticle");
            par.transform.position = transform.position + Vector3.up*1 + Vector3.left * transform.localScale.x * 0.2f;
        }
    }

    private void StartCharging()
    {
        power = (power >= maxPow) ? maxPow : power + chargeRate;
    }

    public bool GetCloseWall()
    {
        return closeWall;
    }
}
