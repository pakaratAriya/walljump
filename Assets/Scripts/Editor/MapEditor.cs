 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

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
      
    static GameObject sceneCam;
    Camera myCam;
    [MenuItem("Window/Map Editor/Enable %e")]
    private static void OpenMapEditor()
    {
        Selection.activeGameObject = FindObjectOfType<Map>().gameObject;
        ActiveEditorTracker.sharedTracker.isLocked = true;
        sceneCam = GameObject.Find("SceneCamera");
        Debug.Log(sceneCam.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0,1,0)));
    }

  [MenuItem("Window/Map Editor/Disable %l")]
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
    }
    else
    {
      DrawTileEditor();
    }
        GUILayout.Label("B - Place an object at the mouse cursor");
        GUILayout.Label("N - Remove an object at the mouse cursor");
        GUILayout.Label("M - Start/Stop painting mode");
        GUILayout.Label("; - Remove last created object");
        GUILayout.Label("Ctrl + L - Quit the tool");
    }

  private void Awake()
  {
        sceneCam = GameObject.Find("SceneCamera");
        myCam = sceneCam.GetComponent<Camera>();
    }

  private void GetCam()
  {
    sceneCam = GameObject.Find("SceneCamera");
    myCam = sceneCam.GetComponent<Camera>();
   
  }

  private void OnSceneGUI()
    {
        Vector3 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        Handles.BeginGUI();
        Vector3 onSceneCursor = new Vector3(Mathf.RoundToInt(spawnPosition.x) - 0.5f, Mathf.RoundToInt(spawnPosition.y) + 0.5f, 0);
        if (selectedPrefab || !editMode)
        {
            GUI.backgroundColor = new Color(0, 1, 0, 0.3f);
        }
        else
        {
            GUI.backgroundColor = new Color(1, 0, 0, 0.3f);
        }

    if (myCam == null)
    {
      GetCam();
    }

        Vector3 camInUnit = myCam.ViewportToWorldPoint(new Vector3(0, 0, 0)) - myCam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        camInUnit.x = myCam.ViewportToWorldPoint(Vector3.one).x - myCam.ViewportToWorldPoint(Vector3.zero).x;
        camInUnit.y = myCam.ViewportToWorldPoint(Vector3.one).y - myCam.ViewportToWorldPoint(Vector3.zero).y;
        camInUnit.x = Mathf.Abs(camInUnit.x);
        camInUnit.y = Mathf.Abs(camInUnit.y);
        Vector2 screenSize;
        screenSize.x = myCam.ViewportToScreenPoint(Vector3.one).x - myCam.ViewportToScreenPoint(Vector3.zero).x;
        screenSize.y = myCam.ViewportToScreenPoint(Vector3.one).y - myCam.ViewportToScreenPoint(Vector3.zero).y;
        const float screenFactor = 0.8f;
        Vector2 oneUnit;
        oneUnit.x = (screenSize.x) / camInUnit.x;
        oneUnit.y = (screenSize.y) / camInUnit.y;
        
        GUI.Box(new Rect(HandleUtility.WorldToGUIPoint(onSceneCursor), oneUnit * screenFactor), "");
        string mode_string = "Map Edit Mode: ";
        mode_string += drawing ? " Paint Mode" : "Normal Mode";
        GUILayout.Box(mode_string);
        if (selectedPrefab == null)
        {
            GUILayout.Box("No prefab selected!");
        }
        else
        {
            GUI.backgroundColor = Color.cyan;
            GUILayout.Box("SelectedPrefab: " + selectedPrefab.name);
            GUI.backgroundColor = Color.green;
            int count = CheckTileNumber(selectedPrefab.name);
            GUILayout.Box(selectedPrefab.name + ": " + count);
        }

        Handles.EndGUI();

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.B)
        {
            spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x), Mathf.RoundToInt(spawnPosition.y), 0);
            if (!HasObject(spawnPosition))
            {
                if (editMode)
                {
                    Spawn(spawnPosition);
                }
                else
                {
                    SpawnDependentTile(spawnPosition);
                }
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.M)
        {
            if (drawing)
            {
                drawing = false;
            } else
            {
                drawing = true;
        SceneView sceneView = (SceneView)SceneView.sceneViews[0];
        sceneView.Focus();
      }
        }

        if (drawing)
        {
            spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x), Mathf.RoundToInt(spawnPosition.y), 0);
            if (!HasObject(spawnPosition))
            {
                if (editMode)
                {
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    Spawn(spawnPosition);
                } else
                {
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    SpawnDependentTile(spawnPosition);
                }
                
            }
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Semicolon)
        {
            if (spawnedGo.Count > 0)
            {
                Vector2 pos = Vector2.zero;
                bool isIndy = false;
                if (spawnedGo[spawnedGo.Count - 1].GetComponent<IndependentBlock>() != null)
                {
                    GameObject go = spawnedGo[spawnedGo.Count - 1];
                   pos = new Vector2(go.transform.position.x, go.transform.position.y);
                    isIndy = true;
                }
                DestroyImmediate(spawnedGo[spawnedGo.Count - 1]);
                if (isIndy)
                {
                    ChangeAroundATile(pos);
                }
                spawnedGo.RemoveAt(spawnedGo.Count - 1);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N)
        {
            spawnPosition = new Vector3((spawnPosition.x), (spawnPosition.y), 0); 
            if (HasObject(spawnPosition))
            {
                Vector2 mouseWorldPosition = new Vector2(spawnPosition.x, spawnPosition.y);
                RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
                Debug.Log("object = " + hitInfo.collider.gameObject.name);
                if (hitInfo.collider.transform.parent.tag == "drawableObject" || hitInfo.collider.transform.tag == "drawableObject"
		|| hitInfo.collider.GetComponent<IndependentBlock>())
                {
                    Vector2 pos = Vector2.zero;
                    bool isIndy = false;
                    if (hitInfo.collider.GetComponent<IndependentBlock>() != null)
                    {
                        GameObject go = hitInfo.collider.gameObject;
                        pos = new Vector2(go.transform.position.x, go.transform.position.y);
                        DestroyImmediate(hitInfo.collider.gameObject);
                        ChangeAroundATile(pos);
                        return;
                    }
		    
                    
                    if (spawnedGo.Contains(hitInfo.collider.transform.parent.gameObject)){
                        spawnedGo.Remove(hitInfo.collider.transform.parent.gameObject);
                    }
                    if (hitInfo.collider.transform.parent.gameObject.GetComponent<Tile>() != null)
                    {
                        DestroyImmediate(hitInfo.collider.transform.parent.gameObject);
                    }
                    else if (hitInfo.collider.GetComponent<Tile>() != null)
                    {
                        DestroyImmediate(hitInfo.collider.transform.gameObject);
                    }
                   
                    //Debug.Log(hitInfo.collider.transform.parent.gameObject.name);
                    //DestroyImmediate(hitInfo.collider.transform.gameObject);
                    if (isIndy)
                    {
                        ChangeAroundATile(pos);
                    }
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                    map = (Map)target;
                }
                
            }
        }

           
    }

    private GameObject SpawnDependentTile(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<IndependentBlock>() != null)
            {
                return null;
            }
        }
        string myTile = "independentTiles/" + map.AssessTile(pos);
        IndependentBlock myObj = Resources.Load<IndependentBlock>(myTile);
        IndependentBlock myGo = (IndependentBlock)PrefabUtility.InstantiatePrefab(myObj);
        myGo.transform.position = new Vector3(pos.x, pos.y, 0);
        myGo.transform.parent = map.transform;
        spawnedGo.Add(myGo.gameObject);

        ChangeAroundATile(pos);
        return myGo.gameObject;
    }

    private void ChangeAroundATile(Vector2 pos)
    {
        for (int i = (int)pos.x - 1; i <= (int)pos.x + 1; i++)
        {
            for (int j = (int)pos.y - 1; j <= (int)pos.y + 1; j++)
            {
                if (i != pos.x || j != pos.y)
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(i, j), Vector2.zero);
                    if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.GetComponent<IndependentBlock>() != null)
                        {
                            int id = 0;
                            bool inSpawnedGo = false;
                            if(spawnedGo.Contains(hitInfo.collider.gameObject))
                            {
                                inSpawnedGo = true;
                                id = spawnedGo.IndexOf(hitInfo.collider.gameObject);
                            }
                                
                            DestroyImmediate(hitInfo.collider.gameObject);
                            if (inSpawnedGo)
                            {
                                spawnedGo.RemoveAt(id);
                            }
                            string newTile = "independentTiles/" + map.AssessTile(new Vector2(i, j));
                            IndependentBlock newObj = Resources.Load<IndependentBlock>(newTile);
                            IndependentBlock newGo = (IndependentBlock)PrefabUtility.InstantiatePrefab(newObj);
                            newGo.transform.position = new Vector3(i, j, 0);
                            newGo.transform.parent = map.transform;
                            if(inSpawnedGo)
                            {
                                spawnedGo.Insert(id, newGo.gameObject);
                            }
                            else
                            {
                                spawnedGo.Add(newGo.gameObject);
                            }
                            

                        }
                    }
                }
            }
        }

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
        GUI.backgroundColor = Color.green;
        Texture prefabTexture = AssetPreview.GetAssetPreview(Resources.Load("independentTiles/N-N"));
        GUILayout.Box(prefabTexture, GUILayout.Width(70), GUILayout.Height(70));
        {
            
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
        
 
    GameObject Spawn(Vector2 _spawnPosition)
    {
        if(selectedPrefab == null)
    {
      return null;
    }
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
            /*
            for(int r = -1; r <= 1; r+=2)
            { 
                HasBlockTile(_spawnPosition + new Vector2(0, r));
                HasBlockTile(_spawnPosition + new Vector2(r, 0));
            }
            */
            Vector2[] scanDirection = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            foreach (Vector2 v in scanDirection)
            {
                RaycastHit2D hit = Physics2D.Raycast(_spawnPosition, v, 1.0f);
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<IndependentBlock>() != null)
                    {
                        float angle = 0;
                        if (v == Vector2.right)
                        {
                            angle = 90;
                        } else if (v == Vector2.up)
                        {
                            angle = 180;
                        } else if (v == Vector2.left)
                        {
                            angle = 270;
                        }
                        GameObject newgo = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                        newgo.transform.eulerAngles = Vector3.zero + Vector3.forward * angle;
                        newgo.transform.position = hit.point;
                        newgo.name = selectedPrefab.name;
                        map = (Map)target;
                        newgo.transform.parent = map.transform;
                        spawnedGo.Add(newgo);

                        return newgo;
                    }
                }
            }
        }
        return null;
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
