using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHelper : MonoBehaviour {

    private Character player;
    public LineRenderer line;
    private void Awake()
    {
        player = FindObjectOfType<Character>();
        player.ch = this;
        line = GetComponent<LineRenderer>();
	if(line){
            print("exist");
        }else{
            print("broken");
        }
    }

    public void DrawTraject(Vector2 start, Vector2 velo)
    {
        line.positionCount = 40;
        Vector2 pos = start;
        Vector2 vel = velo;
        Vector2 grav = Physics.gravity * 1.5f;
        for(int i = 0; i< 40; i++)
        {
            line.SetPosition(i, pos);
            vel = vel += grav * Time.fixedDeltaTime;
            pos = pos += vel * Time.fixedDeltaTime;
        }

    }
}
