using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public List<Enemy> enemies;
    private void OnEnable()
    {
        Invoke("SpawnEnemy", 1);
    }
    private void SpawnEnemy()
    {
        int rand = Random.Range(0, enemies.Count);
        if (enemies.Count != 0)
        {
            Enemy temp = MapManager.SpawnEnemy(enemies[rand].name);
            if (this.name.Contains("Left"))
            {
                temp.transform.position = transform.position + Vector3.right * temp.leftOffset;
                temp.transform.eulerAngles = new Vector3(0, 0, temp.leftAngle);
                temp.transform.localScale = new Vector3(temp.leftScale, 1, 1);
            }
            else
            {
                temp.transform.position = transform.position + Vector3.right * temp.rightOffset;
                temp.transform.eulerAngles = new Vector3(0, 0, temp.rightAngle);
                temp.transform.localScale = new Vector3(temp.rightScale, 1, 1);
            }
            temp.originalScale = temp.transform.localScale.x;
            temp.direction = new Vector3(temp.originalScale, 0, 0);
        }

    }
}
