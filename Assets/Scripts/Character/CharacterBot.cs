using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBot : MonoBehaviour
{
    Character ch;
    private void Awake()
    {
        ch = GetComponent<Character>();
    }
}
