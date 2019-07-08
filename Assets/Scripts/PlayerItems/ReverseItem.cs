using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseItem : InGameItem
{
    List<Unit> allGameObject = new List<Unit>();
    bool recording = false;
    
    Dictionary<Unit, Stack<Vector3>> allGameObjectDic = new Dictionary<Unit, Stack<Vector3>>();

    public void Start()
    {
        allGameObject.AddRange(FindObjectsOfType<Unit>());
        foreach (Unit u in allGameObject)
        {

            allGameObjectDic.Add(u, new Stack<Vector3>());
        }

    }


    public override void TriggerItem()
    {
        if(myChar.closeWall)
            StartCoroutine(StartReverse());
    }

    private void Update()
    {
        if (!myChar.dead)
        {
            if(!myChar.closeWall || myChar.rb.gravityScale != 0)
            {
                StartRecording(); 
            }
            else
            {
                recording = false;
            }
        }
    }

    void StartRecording()
    {
        if (!recording)
        {
            foreach (Unit u in allGameObject)
            {
                if (u)
                {
                    allGameObjectDic[u].Clear();
                }
            }
            recording = true;
        }
        else
        {
            
            foreach(Unit u in allGameObject)
            {
                if (u)
                {
                    allGameObjectDic[u].Push(u.transform.position);
                }
            }
        }
    }

    IEnumerator StartReverse()
    {
        bool reversing = true;
        myChar.transform.localScale = new Vector3(myChar.transform.localScale.x * -1, 1, 1);
        myChar.direction *= -1;
        while (reversing)
        {

            foreach(Unit unit in allGameObjectDic.Keys)
            {
                if (unit)
                {
                    if (allGameObjectDic[unit].Count > 0)
                    {
                        Vector3 pos = allGameObjectDic[unit].Pop();
                        unit.transform.position = pos;
                    }
                }
            }
            if (allGameObjectDic[(Unit)myChar].Count == 0)
            {
                reversing = false;
                myChar.closeWall = true;
            }
           
            yield return new WaitForEndOfFrame();
        }
    }
}
