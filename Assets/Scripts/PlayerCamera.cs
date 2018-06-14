using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    private float camPeek = 0;
    public float offset = 4;
    private int offsetY = 10;
	// Update is called once per frame
	void Update () {
        if (MapManager.player.transform.position.y >= camPeek - 8 && !MapManager.player.dead)
        {
            float speed = 0f;
            int designatedSize = SelectSize();
            transform.position = new Vector3(0, MapManager.player.transform.position.y + offset, -10);
            float currentSize = GetComponent<Camera>().orthographicSize;
            GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(currentSize, 9 + designatedSize, ref speed, Time.deltaTime * 5);

        }
        camPeek = (transform.position.y > camPeek) ? transform.position.y : camPeek;
	}

    int SelectSize()
    {
        int designatedSize = 0;
        designatedSize = (MapManager.CheckCamPos(offsetY) > MapManager.CheckCamPos(-offsetY)) ?
            MapManager.CheckCamPos(offsetY) : MapManager.CheckCamPos(-offsetY);
        designatedSize = (designatedSize > MapManager.CheckCamPos(0)) ?
            designatedSize : MapManager.CheckCamPos(0);
        return designatedSize;
        
    }
}
