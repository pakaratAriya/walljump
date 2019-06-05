using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Unit spawnPref;
    public Unit unit;
    public Camera myCam;
    private bool alreadySpawned = false;
    public enum SpawnPoint
    {
        Top,
        Bottom
    }
    private Vector3 spawnPos;
    public SpawnPoint spawnPoint;

    private void Start()
    {
        myCam = FindObjectOfType<Camera>();
        unit = Instantiate<Unit>(spawnPref);
        unit.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        

    }

    public void Spawn()
    {
        
        float x = Random.Range(0f, 1f) > 0.5f ? -2.25f : 2.25f;
        float y = spawnPoint == SpawnPoint.Top ? 10 : -10;
        spawnPos = new Vector3(x, y, 0);
        unit.transform.position = spawnPos;
        unit.gameObject.SetActive(true);
        alreadySpawned = false;
    }

    public void Despawn()
    {
        alreadySpawned = true;
        unit.gameObject.SetActive(false);
        SpawnManager.ResetAlreadySpawn();
    }

    private void Update()
    {
        if (alreadySpawned)
        {
            return;
        }
        if(spawnPoint == SpawnPoint.Bottom)
        {
            if (unit.transform.position.y > myCam.ViewportToWorldPoint(Vector3.one).y + 1)
            {
                Despawn();
            }
        }
        else
        {
            if (unit.transform.position.y < myCam.ViewportToWorldPoint(Vector3.zero).y - 1)
            {
                Despawn();
            }
        }
    }
}
