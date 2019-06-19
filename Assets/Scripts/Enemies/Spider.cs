using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy {
    Vector3 startPoint;
    Vector3 endPoint;
    public MoveDirection modeDirection;
    public float distance = 3f;
    public float speed = 3f;
    SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        startPoint = transform.position;
        if(modeDirection == MoveDirection.up)
        {
            endPoint = startPoint + Vector3.up * distance;
            transform.eulerAngles = Vector3.forward * 90;
        }
        else
        {
            endPoint = startPoint + Vector3.right * distance;
        }
        StartCoroutine(MoveSpider());
	}

    IEnumerator MoveSpider()
    {
        bool plus = true;
        if(modeDirection == MoveDirection.up)
        {
            while (true)
            {
                if (plus)
                {
                    transform.Translate(Time.deltaTime * speed,0 , 0);
                    sr.flipX = false;
                }
                else
                {
                    transform.Translate(-Time.deltaTime * speed, 0, 0);
                    sr.flipX = true;
                }
                if (plus && transform.position.y > endPoint.y)
                    plus = false;
                if (!plus && transform.position.y < startPoint.y)
                    plus = true;
                yield return new WaitForFixedUpdate();
            }
            
        }
        else
        {
            while (true)
            {
                if (plus)
                {
                    transform.Translate(Time.deltaTime * speed, 0 , 0);
                    sr.flipX = false;
                }
                else
                {
                    transform.Translate(-Time.deltaTime * speed, 0, 0);
                    sr.flipX = true;
                }
                if (plus && transform.position.x > endPoint.x)
                    plus = false;
                if (!plus && transform.position.x < startPoint.x)
                    plus = true;
                yield return new WaitForFixedUpdate();
            }
        }
        
    }


}

public enum MoveDirection
{
    up,
    right
}