using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {
    GameObject[] prefabs;
    GameObject selectedPrefab;
    List<GameObject> spawnedGo = new List<GameObject>();
    bool drawing = false;

    [MenuItem("Window/Map Editor/Enable %E")]
    private static void OpenMapEditor()
    {
        Selection.activeGameObject = FindObjectOfType<Map>().gameObject;
        ActiveEditorTracker.sharedTracker.isLocked = true;
    }

    [MenuItem("Window/Map Editor/Disable %Q")]
    private static void DisableMapEditor()
    {
        ActiveEditorTracker.sharedTracker.isLocked = false;
        Selection.activeGameObject = null;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        List<Object> obj = new List<Object>();
        obj.AddRange(Resources.LoadAll("Tiles1"));
        obj.AddRange(Resources.LoadAll("Obstacle1"));
        obj.AddRange(Resources.LoadAll("Items"));
        prefabs = new GameObject[obj.Count];
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabs[i] = (GameObject)obj[i];
        }
        GUILayout.BeginHorizontal();
        if (prefabs != null)
        {
            int elementInThisRows = 0;
            for (int i = 0; i < prefabs.Length; i++)
            {
                elementInThisRows++;
                Texture prefabTexture = AssetPreview.GetAssetPreview(prefabs[i]);
                if (prefabs[i] == selectedPrefab)
                {
                    GUI.backgroundColor = Color.green;
                } else
                {
                    GUI.backgroundColor = Color.grey;
                }
                
                if (GUILayout.Button(prefabTexture, GUILayout.MaxWidth(70), GUILayout.MaxHeight(70)))
                {
                    selectedPrefab = prefabs[i];
                    EditorWindow.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
                }
                if (elementInThisRows > Screen.width / 100)
                {
                    elementInThisRows = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        Vector3 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.E)
        {
            spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x), Mathf.RoundToInt(spawnPosition.y), 0);
            if (!HasObject(spawnPosition))
            Spawn(spawnPosition);
        }
        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.R)
        {
            if (drawing)
            {
                drawing = false;
            } else
            {
                drawing = true;
            }
        }

        if (drawing)
        {
            spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x), Mathf.RoundToInt(spawnPosition.y), 0);
            if (!HasObject(spawnPosition))
            {
                Spawn(spawnPosition);
            }
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X)
        {
            if (spawnedGo.Count > 0)
            {
                DestroyImmediate(spawnedGo[spawnedGo.Count - 1]);
                spawnedGo.RemoveAt(spawnedGo.Count - 1);
            }
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
        {
            spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x), Mathf.RoundToInt(spawnPosition.y), 0);
            if (HasObject(spawnPosition))
            {
                Vector2 mouseWorldPosition = new Vector2(spawnPosition.x, spawnPosition.y);
                RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
                spawnedGo.Remove(hitInfo.collider.gameObject);
                DestroyImmediate(hitInfo.collider.gameObject);
            }
        }

        Handles.BeginGUI();
        GUILayout.Box("Map Edit Mode");
        if (selectedPrefab == null)
        {
            GUILayout.Box("No prefab selected!");
        } else
        {
            GUI.backgroundColor = Color.cyan;
            GUILayout.Box("SelectedPrefab: " + selectedPrefab.name);
            GUI.backgroundColor = Color.green;
            int count = CheckTileNumber(selectedPrefab.name);
            GUILayout.Box(selectedPrefab.name + ": " + count);
        }

        Handles.EndGUI();      
    }

    private int CheckTileNumber(string chkName)
    {
        Map map = (Map)target;
        Tile[] tile = map.GetComponentsInChildren<Tile>();
        int count = 0;
        foreach (Tile t in tile)
        {
            if (t.name == selectedPrefab.name)
            {
                count++;
            }
        }
        return count;
    }
        
 
    void Spawn(Vector2 _spawnPosition)
    {
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        go.transform.position = new Vector3(_spawnPosition.x, _spawnPosition.y, 0);

        go.name = selectedPrefab.name;
        Map map = (Map)target;
        go.transform.parent = map.transform;
        spawnedGo.Add(go);
        if (go.GetComponentInChildren<Savepoint>() != null)
        {
            int count = CheckTileNumber(go.name);
            go.GetComponentInChildren<Savepoint>().saveNum = count;
        }
    }

    private bool HasObject(Vector3 spawnPosition)
    {
        Vector2 mouseWorldPosition = new Vector2(spawnPosition.x, spawnPosition.y);
        RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hitInfo.collider != null)
        {
            return true;
        }
        return false;
    }
}
