using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    private float camPeek = 0;
	// Update is called once per frame
	void Update () {
        if (MapManager.player.transform.position.y >= camPeek - 8 && !MapManager.player.dead)
        {
            transform.position = new Vector3(0, MapManager.player.transform.position.y + 2, -10);
        }
        camPeek = (transform.position.y > camPeek) ? transform.position.y : camPeek;
	}
}
