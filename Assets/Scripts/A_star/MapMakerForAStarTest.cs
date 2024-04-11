using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar;

public class MapMakerForAStarTest : MonoBehaviour
{
    [Header("Board Info")]
    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private int destX;

    [SerializeField]
    private int destY;

    [SerializeField]
    private MapTile tilePrefab;

    public Node[,] NodeArray;

    private List<Node> Path;

    private void Awake()
    {
        NodeArray = new Node[width, height];
    }
    private void Start()
    {
        GenerateGrid();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PathFinder pathfinder = new PathFinder(NodeArray, new Vector2Int(0, 0), new Vector2Int(destX, destY), 5);
            Path = pathfinder.PathFinding();
            showPath();
        }
    }

    private void GenerateGrid()
    {
        bool isBlocked;
        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                // 타일 생성
                MapTile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, gameObject.transform);
                spawnedTile.name = $"Tile({x}, {y})";

                if ((x == 0 && y == 4) ||
                    (x == 0 && y == 2) ||
                    (x == 1 && y == 2) ||
                    (x == 2 && y == 1) ||
                    (x == 2 && y == 2) ||
                    (x == 4 && y == 1) ||
                    (x == 4 && y == 2) ||
                    (x == 4 && y == 3) ||
                    (x == 4 && y == 4) ||
                    (x == 1 && y == 4) ||
                    (x == 2 && y == 4) ||
                    (x == 3 && y == 4) ||
                    (x == 4 && y == 4)) { isBlocked = true; }
                else { isBlocked = false; }
                // 노드 추가
                NodeArray[x, y] = new Node(isBlocked, x, y);
            }
        }
    }
    void showPath()
    {
        for (int i = 0; i < Path.Count; i++)
        {
            Debug.Log(i + "번째 경로: " + Path[i].x + ", " + Path[i].y);
        }
    }
}