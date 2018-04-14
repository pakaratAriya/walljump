using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int updateLength = 10;
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= MapManager.player.transform.position.y - updateLength)
        {
            MapManager.ChangePosition(this);
        }
	}

}
