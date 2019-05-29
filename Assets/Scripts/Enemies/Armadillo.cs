using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armadillo : Enemy
{
    private Sprite idleSpr;
    [SerializeField]
    private Sprite spinningSpr;
    private SpriteRenderer sr;
    public float waitTime = 1;
    public Vector2 jumpSpeed = new Vector2(10, 10);
    private Coroutine armadilloMove;
    private bool atWall = true;
    private float spinningSpeed = 5000f;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, 270);
        sr = GetComponent<SpriteRenderer>();
        idleSpr = sr.sprite;
        armadilloMove = StartCoroutine(ArmadilloMoving());
    }

    private void Update()
    {
        if(sr.sprite == spinningSpr){
            transform.Rotate(0, 0, Time.deltaTime * 
	    (rb.velocity.x > 0?spinningSpeed:-spinningSpeed));
        }
    }

    IEnumerator ArmadilloMoving(){
	{
	    while(true){
                yield return new WaitUntil(() => atWall);
                yield return new WaitForSeconds(waitTime);
                PerformJump();
            }  
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Tile"){
            rb.gravityScale = 0;
            sr.sprite = idleSpr;
            rb.velocity = Vector2.zero;
	    // rotate the body to 90 degree or 270 degree depends on the wall position
            transform.eulerAngles = new Vector3(0, 0,
            col.transform.position.x > transform.position.x ? 90 : 270);
            sr.flipX = transform.eulerAngles.z == 90 ? true : false;
            atWall = true;
        }
    }

    void PerformJump(){
        rb.velocity = 
	new Vector2(transform.eulerAngles.z==270?
	    jumpSpeed.x:
	    -jumpSpeed.x
	    , jumpSpeed.y);
        sr.sprite = spinningSpr;
        rb.gravityScale = 1;
        atWall = false;
    }
}
