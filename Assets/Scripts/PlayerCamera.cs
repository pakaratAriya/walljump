using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0, MapManager.player.transform.position.y + 2, -10);
	}
}
