using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level section", menuName = "Create level section", order = 2)]
[System.Serializable]
public class LevelSection : ScriptableObject {
    public int startPoint = 0;
    public int endPoint = 100;
    public int gapSize = 3;
    public float enemiesChance = 30;
    public enemy[] enemies;
}

[System.Serializable]
public struct enemy{
    public string name;
    public float chance;
}
