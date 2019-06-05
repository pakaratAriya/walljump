using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<CharacterSpawner> spawners;
    private int index = 0;
    public static bool alreadySpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCharacter());
    }

    public static void ResetAlreadySpawn()
    {
        alreadySpawn = false;
    }

    IEnumerator SpawnCharacter()
    {
        while (true)
        {
            yield return new WaitUntil(() => !alreadySpawn);
            CallNewCharacter();
            alreadySpawn = true;
        }
    }

    public void CallNewCharacter()
    {
        int oldIndex = index;
        if (spawners.Count > 1)
        {
            while(oldIndex == index)
                index = Random.Range(0, spawners.Count);
        }
        spawners[index].Spawn();
        
    }

}
