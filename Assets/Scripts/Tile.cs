using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int updateLength = 15;
	// Update is called once per frame
	void Update () {
        if (!MapManager.player)
        {
            return;
        }
		if (transform.position.y <= MapManager.player.transform.position.y - updateLength)
        {
            MapManager.Despawn(this);
        }
	}

}
