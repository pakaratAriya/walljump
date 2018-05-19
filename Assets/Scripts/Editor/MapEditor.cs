using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {
    GameObject[] prefabs;
    GameObject selectedPrefab;
    List<GameObject> spawnedGo = new List<GameObject>();
    static bool enable = false;
    bool drawing = false;

    [MenuItem("Window/Map Editor/Enable %E")]
    private static void OpenMapEditor()
    {
        enable = true;
        Selection.activeGameObject = FindObjectOfType<Map>().gameObject;
        ActiveEditorTracker.sharedTracker.isLocked = true;
    }

    [MenuItem("Window/Map Editor/Disable %Q")]
    private static void DisableMapEditor()
    {
        ActiveEditorTracker.sharedTracker.isLocked = false;
        enable = false;
        Selection.activeGameObject = null;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Object[] obj = Resources.LoadAll("Map");
        prefabs = new GameObject[obj.Length];
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
        if (selectedPrefab != null)
        {
            if (selectedGameObject != null)
            {
                float selectedGameObjectWidth = selectedGameObject.GetComponent<Collider2D>().bounds.size.x;
                float selectedGameObjectHeight = selectedGameObject.GetComponent<Collider2D>().bounds.size.z;
                float selectedPrefabWidth = selectedPrefab.GetComponent<Collider2D>().bounds.size.x;
                float selectedPrefabHeight = selectedPrefab.GetComponent<Collider2D>().bounds.size.y;
                if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.W)
                {
                    spawnPosition = new Vector3(selectedGameObject.transform.position.x,
                        selectedGameObject.transform.position.y + selectedGameObjectHeight / 2 + selectedPrefabHeight / 2, 0);
                    Spawn(spawnPosition);
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
            Map map = (Map)target;
            GUI.backgroundColor = Color.green;
            Tile[] tile = map.GetComponentsInChildren<Tile>();
            int count = 0;
            foreach (Tile t in tile)
            {
                if (t.name == selectedPrefab.name)
                {
                    count++;
                }
            }
            GUILayout.Box(selectedPrefab.name + ": " + count);
        }

        Handles.EndGUI();
            
    }
 
    GameObject selectedGameObject;
    void Spawn(Vector2 _spawnPosition)
    {
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        go.transform.position = new Vector3(_spawnPosition.x, _spawnPosition.y, 0);

        selectedGameObject = go;
        go.name = selectedPrefab.name;
        Map map = (Map)target;
        go.transform.parent = map.transform;
        spawnedGo.Add(go);
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
