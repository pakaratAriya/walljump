using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new level", menuName ="Create Level", order = 1)]
public class LevelData : ScriptableObject {
    public List<LevelSection> levelSection;
}
