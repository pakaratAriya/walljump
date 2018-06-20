 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {
    GameObject[] prefabs;
    GameObject[] indTiles;
    GameObject selectedPrefab;
    bool editMode = true;
    Map map;
    List<GameObject> spawnedGo = new List<GameObject>();
    List<MovingDirection> movingDir = new List<MovingDirection>()
    {
        new MovingDirection("Left", Vector3.right * 0.3f, Vector3.forward * 90),
        new MovingDirection("Right", Vector3.right * -0.3f, Vector3.forward * -90),
        new MovingDirection("Floor", Vector3.up * 0.5f, Vector3.zero),
        new MovingDirection("Ceiling", Vector3.up * -0.5f, Vector3.zero),
        new MovingDirection("RightO", Vector3.up * 0.5f, Vector3.zero),
        new MovingDirection("LeftO", Vector3.up * 0.5f, Vector3.zero),
        new MovingDirection("RightI", Vector3.up * -0.5f, Vector3.zero),
        new MovingDirection("LeftI", Vector3.up * -0.5f, Vector3.zero),
    };
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
        map = (Map)target;
        DrawDefaultInspector();
        List<Object> obj = new List<Object>();
        obj.AddRange(Resources.LoadAll("Tiles1"));
        obj.AddRange(Resources.LoadAll("Obstacle1"));
        obj.AddRange(Resources.LoadAll("Items"));
        prefabs = new GameObject[obj.Count];
        indTiles = (Resources.LoadAll<GameObject>("independentTiles"));
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabs[i] = (GameObject)obj[i];
        }
        if(GUILayout.Button("Draw Level"))
        {
            editMode = !editMode;
        }
        if (editMode)
        {
            DrawItems();
        } else
        {
            DrawTileEditor();
        }

        
        
        GUILayout.Label("E - Place an object at the mouse cursor");
        GUILayout.Label("C - Remove an object at the mouse cursor");
        GUILayout.Label("R - Start/Stop painting mode");
        GUILayout.Label("X - Remove last created object");
        GUILayout.Label("Ctrl + Q - Quit the tool");
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
            spawnPosition = new Vector3((spawnPosition.x), (spawnPosition.y), 0); 
            if (HasObject(spawnPosition))
            {
                Vector2 mouseWorldPosition = new Vector2(spawnPosition.x, spawnPosition.y);
                RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
                if (spawnedGo.Contains(hitInfo.collider.gameObject))
                {
                    spawnedGo.Remove(hitInfo.collider.gameObject);
                    DestroyImmediate(hitInfo.collider.gameObject);
                    Map map = (Map)target;
                }
                
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

    private void DrawItems()
    {
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
                }
                else
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

    private void DrawTileEditor()
    {
        GUILayout.BeginHorizontal();

        if (indTiles != null)
        {
            int elementInThisRows = 0;
            for (int i = 0; i < indTiles.Length; i++)
            {
                elementInThisRows++;
                Texture prefabTexture = AssetPreview.GetAssetPreview(indTiles[i]);
                if (indTiles[i] == selectedPrefab)
                {
                    GUI.backgroundColor = Color.green;
                }
                else
                {
                    GUI.backgroundColor = Color.grey;
                }
                GUILayout.Box(prefabTexture, GUILayout.Width(70), GUILayout.Height(70));
                {
                    GUILayout.Button(prefabTexture, GUILayout.Width(10), GUILayout.Height(10));
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
        if (go.GetComponent<WallObject>() != null)
        {
            spawnedGo.Remove(go);
            DestroyImmediate(go);
            for(int r = -1; r <= 1; r+=2)
            { 
                HasBlockTile(_spawnPosition + new Vector2(0, r));
                HasBlockTile(_spawnPosition + new Vector2(r, 0));
            }
        }
    }

    private Vector2 HasBlockTile(Vector2 _spawnPosition)
    {
        Vector2 spawnPos = Vector2.zero;
        RaycastHit2D hitInfo = Physics2D.Raycast(_spawnPosition, Vector2.zero);
        if (hitInfo.collider != null)
        {
            foreach (MovingDirection obj in movingDir)
            {
                if (hitInfo.collider.name.Contains(obj.name))
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(_spawnPosition + (Vector2)obj.shiftPos, Vector2.zero);
                    if (hit2.collider != null)
                    {
                        if ((Vector2)hit2.collider.transform.position == (_spawnPosition + (Vector2)obj.shiftPos))
                        {
                            continue;
                        }
                    }
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                    go.transform.eulerAngles = obj.shiftAngle;
                    go.transform.position = hitInfo.collider.transform.position + obj.shiftPos;
                    go.name = selectedPrefab.name;
                    Map map = (Map)target;
                    go.transform.parent = map.transform;
                    spawnedGo.Add(go);
                }
            }
        }
        return spawnPos;
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
 public class MovingDirection
{
    public Vector3 shiftAngle;
    public Vector3 shiftPos;
    public string name;
    public MovingDirection(string name, Vector3 pos, Vector3 angle)
    {
        this.name = name;
        this.shiftPos = pos;
        this.shiftAngle = angle;
    }
}