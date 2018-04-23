using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGuage : MonoBehaviour {

    public Character character;
    SpriteRenderer sr;
    float alpha = 0f;
    Vector3 oScale;
    float belowDistance = 0.5f;
    float offsetDistance = 0.4f;
	// Use this for initialization
	void Start () {
        character = FindObjectOfType<Character>();
        sr = GetComponent<SpriteRenderer>();
        oScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = character.transform.position - Vector3.up * belowDistance - Vector3.right * offsetDistance;
		if (character.charging)
        {
            alpha = (alpha <= 1) ? alpha + 0.05f : 1;
        } else
        {
            alpha = (alpha >= 0) ? alpha - 0.1f : 0;
        }
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (name == "guage" && character.charging)
        {
            transform.localScale = new Vector3(oScale.x * character.power / character.maxPow, oScale.y);
        }
    }
}
