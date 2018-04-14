using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public static Character player;
    private List<Tile> tileList = new List<Tile>();
    private List<Tile> obstacleList = new List<Tile>();
    private static Tile bgTile;

    private static Dictionary<string, Tile> tileBook = new Dictionary<string, Tile>();
    private static Dictionary<string, Stack<Tile>> tilePool = new Dictionary<string, Stack<Tile>>();

    public static int halfGap = 3;
    public static int upperBoundary = 30;
    public int level = 1;
    public float obsChance = 0.03f;
    public static int currentPoint = -10;
    public static MapManager findMap;

    private void Awake()
    {
        player = FindObjectOfType<Character>();
        findMap = FindObjectOfType<MapManager>();
    }

    // Use this for initialization
    void Start() {
        tileList.AddRange(Resources.LoadAll<Tile>("Tiles" + level));
        obstacleList.AddRange(Resources.LoadAll<Tile>("Obstacle" + level));
        bgTile = Resources.Load<Tile>("Background" + level + "/background");
        tileBook.Add(bgTile.name, bgTile);
        tilePool[bgTile.name] = new Stack<Tile>();
        foreach (Tile tile in tileList)
        {
            tileBook.Add(tile.name, tile);
            tilePool[tile.name] = new Stack<Tile>();
        }
        foreach (Tile tile in obstacleList)
        {
            tileBook.Add(tile.name, tile);
            tilePool[tile.name] = new Stack<Tile>();
        }
        for(int i = -10; i < upperBoundary; i += 10)
        {
            Tile bg = Spawn("background");
            bg.transform.position = new Vector3(0, i, 0);
            bg.transform.SetParent(transform);
            bg.name = "background";
        }
        for (; currentPoint < upperBoundary; currentPoint++)
        {
            for (int j = -20; j <= 20; j++)
            {
                Tile tile = null;
                if (j < -halfGap || j > halfGap)
                {
                    tile = Spawn("BlockTile");
                    tile.name = "BlockTile";
                } else if (j == -halfGap) {
                    tile = Spawn("BlockLeft");
                    tile.name = "BlockLeft";
                } else if (j == halfGap)
                {
                    tile = Spawn("BlockRight");
                    tile.name = "BlockRight";
                }
                if (tile != null)
                {
                    tile.transform.position = new Vector3(j, currentPoint, 0);
                    tile.transform.SetParent(transform);
                }

            }
        }
	}

    private void Update()
    {
        if (player.transform.position.y > 40 && player.transform.position.y < 80)
        {
            halfGap = 4;
        } else
        {
            halfGap = 3;
        }
        if (currentPoint - player.transform.position.y <= upperBoundary)
        {
            ChangePosition();
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
                if (j < -halfGap || j > halfGap)
                {
                    whatToSpawn = "BlockTile";
                }
                else if (j == -halfGap)
                {
                    float gen = Random.Range(0f, 100f);
                    whatToSpawn = (gen <= findMap.obsChance) ? "SpikeLeft" : "BlockLeft";
                }
                else if (j == halfGap)
                {
                    float gen = Random.Range(0f, 100f);
                    whatToSpawn = (gen <= findMap.obsChance) ? "SpikeRight" : "BlockRight";
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
}
