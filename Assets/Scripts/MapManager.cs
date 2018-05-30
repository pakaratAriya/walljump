using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour {
    public static Character player;
    private List<Tile> tileList = new List<Tile>();
    private List<Tile> obstacleList = new List<Tile>();
    private List<Tile> pickupList = new List<Tile>();
    private static Tile bgTile;
    private static List<Tile> obsLeft = new List<Tile>();
    private static List<Tile> obsRight = new List<Tile>();
    private static List<Enemy> enemies = new List<Enemy>();

    private static Dictionary<string, Tile> tileBook = new Dictionary<string, Tile>();
    private static Dictionary<string, Stack<Tile>> tilePool = new Dictionary<string, Stack<Tile>>();
    private static Dictionary<string, Enemy> enemyBook = new Dictionary<string, Enemy>();
    private static Dictionary<string, Stack<Enemy>> enemyPool = new Dictionary<string, Stack<Enemy>>();

    public static int upperBoundary = 30;
    public int level = 1;
    public float obsChance = 0.03f;
    public static int currentPoint = -10;
    public static MapManager findMap;
    public bool autoGenerate = true;
    public static float startGap = 3;
    public static float endGap = 3;
    // use to manipulate creating leftTile when CurveLeftIn
    private static bool leftDelay = false;
    private static bool rightDelay = false;
    private static Vector3 playerStartPoint = new Vector3(-2.55f, 0, 0);
    private static int playerStartScale = 1;
    private static Vector2 savingGap = new Vector2(3, 3);
    private static List<int> savedList;

    public static int playerLives = 4;

    private void Awake()
    {
        if (savedList == null)
        {
            savedList = new List<int>();
        }
        player = FindObjectOfType<Character>();
        findMap = FindObjectOfType<MapManager>();
    }

    public static void ResetValue()
    {
        PoolManager.ResetValue();
        obsLeft.Clear();
        obsRight.Clear();
        tileBook.Clear();
        tilePool.Clear();
        enemyBook.Clear();
        enemyPool.Clear();
        upperBoundary = 30;
        currentPoint = -10;
        

    }

    // Use this for initialization
    void Start() {
        tileList.AddRange(Resources.LoadAll<Tile>("Tiles" + level));
        obstacleList.AddRange(Resources.LoadAll<Tile>("Obstacle" + level));
        enemies.AddRange(Resources.LoadAll<Enemy>("Enemies"));
        pickupList.AddRange(Resources.LoadAll<Tile>("Items"));
        bgTile = Resources.Load<Tile>("Background" + level + "/background");
        if (!tileBook.ContainsKey(bgTile.name))
            tileBook.Add(bgTile.name, bgTile);
        tilePool[bgTile.name] = new Stack<Tile>();
        for (int i = -10; i < upperBoundary; i += 10)
        {
            Tile bg = Spawn("background");
            bg.transform.position = new Vector3(0, i, 0);
            bg.transform.SetParent(transform);
            bg.name = "background";
        }
        foreach (Tile tile in obstacleList)
        {
            if (tile.name.Contains("Left"))
            {
                obsLeft.Add(tile);
            } else if (tile.name.Contains("Right"))
            {
                obsRight.Add(tile);
            }
        }
        foreach (Tile tile in tileList)
        {
            if (!tileBook.ContainsKey(tile.name))
            {
                tileBook.Add(tile.name, tile);
                tilePool[tile.name] = new Stack<Tile>();
            }
            
        }
        foreach (Tile tile in obstacleList)
        {
            if (!tileBook.ContainsKey(tile.name))
            {
                tileBook.Add(tile.name, tile);
                tilePool[tile.name] = new Stack<Tile>();
            }
        }

        foreach (Tile tile in pickupList)
        {
            if (!tileBook.ContainsKey(tile.name))
            {
                tileBook.Add(tile.name, tile);
                tilePool[tile.name] = new Stack<Tile>();
            }
        }

        foreach (Enemy enemy in enemies)
        {
            if (!enemyBook.ContainsKey(enemy.name))
            {
                enemyBook.Add(enemy.name, enemy);
                enemyPool[enemy.name] = new Stack<Enemy>();
            }
        }
        LoadPlayerSavePoint();
        currentPoint = Mathf.FloorToInt(player.transform.position.y) - 10;
        InitiateLevel();
        
	}

    private void InitiateLevel()
    {
        startGap = savingGap.x;
        endGap = savingGap.y;
        Savepoint[] savepoints = FindObjectsOfType<Savepoint>();
        for (int i = 0; i < savepoints.Length; i++)
        {
            if (savedList.Contains(savepoints[i].saveNum))
            {
                savepoints[i].saved = true;
            }
        }
        for (; currentPoint < upperBoundary; currentPoint++)
        {
            for (int j = -20; j <= 20; j++)
            {
                bool canSpawn = CanSpawn(new Vector2(j, currentPoint));
                Tile tile = null;
                if ((j < -startGap || j > endGap) && canSpawn)
                {
                    tile = Spawn("BlockTile");
                }
                else if (j == -startGap && canSpawn && !leftDelay)
                {
                    tile = Spawn("BlockLeft");
                }
                else if (j == endGap && canSpawn)
                {
                    if (rightDelay)
                    {
                        tile = Spawn("BlockTile");
                    }
                    else
                    {
                        tile = Spawn("BlockRight");
                    }

                }
                if (tile != null)
                {
                    tile.transform.position = new Vector3(j, currentPoint, 0);
                    tile.transform.SetParent(transform);
                }

            }
            leftDelay = false;
            rightDelay = false;
        }
    }

    private void Update()
    {
        if (currentPoint - player.transform.position.y <= upperBoundary)
        {
            if (autoGenerate)
            {
                ChangePosition();
            } else
            {
                SetPosition();
            }
            
        }
    }

    public static void ChangePosition()
    {
        while (currentPoint <= player.transform.position.y + upperBoundary)
        {
            for (int j = -20; j <= 20; j++)
            {
                string whatToSpawn = "";
                Tile tile = null;
                if (j < -startGap || j > endGap)
                {
                    whatToSpawn = "BlockTile";
                }
                else if (j == -startGap)
                {
                    float gen = Random.Range(0f, 100f);
                    int rand = Random.Range(0, obsLeft.Count);
                    whatToSpawn = (gen <= findMap.obsChance) ? obsLeft[rand].name : "BlockLeft";
                }
                else if (j == endGap)
                {
                    float gen = Random.Range(0f, 100f);
                    int rand = Random.Range(0, obsLeft.Count);
                    whatToSpawn = (gen <= findMap.obsChance) ? obsRight[rand].name : "BlockRight";
                }
                if (whatToSpawn != "")
                {
                    tile = Spawn(whatToSpawn);
                    tile.transform.position = new Vector3(j, currentPoint, 0);
                    tile.transform.SetParent(findMap.transform);
                }
            }
            if (currentPoint % 10 == 0)
            {
                Tile bg = Spawn("background");
                bg.transform.position = new Vector3(0, currentPoint, 0);
                bg.transform.SetParent(findMap.transform);
            }
            currentPoint++;
        }
    }

    public static void SetPosition()
    {
        while (currentPoint <= player.transform.position.y + upperBoundary)
        {
            
            for (int j = -20; j <= 20; j++)
            {
                bool canSpawn = CanSpawn(new Vector2(j, currentPoint));
                string whatToSpawn = "";
                Tile tile = null;
                if ((j < -startGap || j > endGap) && canSpawn)
                {
                    whatToSpawn = "BlockTile";
                }
                else if (j == -startGap && !leftDelay && canSpawn)
                {
                    whatToSpawn = "BlockLeft";
                    
                }
                else if (j == endGap && canSpawn)
                {
                    if (rightDelay)
                    {
                        whatToSpawn = "BlockTile";
                    } else
                    {
                        whatToSpawn = "BlockRight";
                    }
                }
                if (whatToSpawn != "")
                {
                    tile = Spawn(whatToSpawn);
                    tile.transform.position = new Vector3(j, currentPoint, 0);
                    tile.transform.SetParent(findMap.transform);
                }
            }
            if (currentPoint % 10 == 0)
            {
                Tile bg = Spawn("background");
                bg.transform.position = new Vector3(0, currentPoint, 0);
                bg.transform.SetParent(findMap.transform);
            }
            leftDelay = false;
            rightDelay = false;
            currentPoint++;
        }
    }

    public static Tile Spawn(string name)
    {
        Tile tile = null;
        if (tilePool[name].Count <= 1)
        {
            tile = Instantiate<Tile>(tileBook[name]);
            tile.name = name;
        }
        else
        {
            tile = tilePool[name].Pop();
            tile.gameObject.SetActive(true);
        }
        tile.transform.SetParent(findMap.transform);
        return tile;
    }

    public static void Despawn(Tile tile)
    {
        tilePool[tile.name].Push(tile);
        tile.gameObject.SetActive(false);
    }

    public static Enemy SpawnEnemy(string name)
    {
        Enemy enemy = null;
        if (enemyPool[name].Count <= 1)
        {
            enemy = Instantiate<Enemy>(enemyBook[name]);
            enemy.name = name;
        }
        else
        {
            enemy = enemyPool[name].Pop();
            enemy.gameObject.SetActive(true);
        }
        enemy.transform.SetParent(findMap.transform);
        return enemy;
    }

    public static void DespawnEnemy(Enemy enemy)
    {
        enemyPool[enemy.name].Push(enemy);
        enemy.gameObject.SetActive(false);
    }

    public static bool CanSpawn(Vector2 pos)
    {
        bool canSpawn = true;
        if (Physics2D.OverlapPoint(pos) != null)
        {
            canSpawn = false;
            /// check if the object is a turning point or not
            /// if it is, change the start point
            /// 
            string detectedObj = Physics2D.OverlapPoint(pos).name;
            if (detectedObj.Contains("LeftIn"))
            {
                startGap--;
                leftDelay = true;
            } else if (detectedObj.Contains("LeftOut"))
            {
                startGap++;
            } else if (detectedObj.Contains("CurveRightIn"))
            {
                endGap--;
            } else if (detectedObj.Contains("CurveRightOut"))
            {
                endGap++;
                rightDelay = true;
            }
        }
        return canSpawn;
    }

    public static void PlayerDie()
    {
        playerLives--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetValue();
    }

    public static void SetPlayerSavePoint(Savepoint savepoint, float x, float y, string side)
    {
        playerStartPoint = new Vector3(x, y, 0);
        if (side.Contains("left"))
        {
            playerStartPoint.x += 0.5f;
            playerStartScale = 1;
        }
        else
        {
            playerStartPoint.x -= 0.5f;
            playerStartScale = -1;
        }
        findMap.CheckGapAt(y - 9);
        savedList.Add(savepoint.saveNum);
    }

    public static void LoadPlayerSavePoint()
    {
        player.direction = playerStartScale;
        player.transform.localScale = new Vector3(playerStartScale, 1, 1);
        player.transform.position = playerStartPoint;
    }

    private void CheckGapAt(float y)
    {
        bool hasTile = true;
        for (int col = -20; col <= 20; col++)
        {
            Vector2 checkPos = new Vector2(col, y);
            RaycastHit2D hitInfo = Physics2D.Raycast(checkPos, Vector2.zero);
            print(checkPos);
            if (hitInfo.collider != null)
            {
                print(hitInfo.collider.name);
                if (hitInfo.collider.name.Contains("Right") && hasTile == false)
                {
                    savingGap.y = col;
                   
                }
                else if (hitInfo.collider.name.Contains("Left") && hasTile == true)
                {
                    savingGap.x = -col;
                    hasTile = false;
                    
                }
            }
            
        }
        print(savingGap);
        
    }
}
