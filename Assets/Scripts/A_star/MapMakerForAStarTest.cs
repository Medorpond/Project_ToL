using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

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

    private PathFinder pathfinder;

    private void Awake()
    {
        NodeArray = new Node[width, height];
    }
    private void Start()
    {
        GenerateGrid();
        pathfinder = PathFinder.GetInstance(); // �̱��� �ν��Ͻ� ����
        pathfinder.Init(NodeArray); // �ʱ�ȭ
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Path = pathfinder.PathFinding(new Vector2Int(0, 0), new Vector2Int(destX, destY));
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
                // Ÿ�� ����
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
                // ��� �߰�
                NodeArray[x, y] = new Node(isBlocked, x, y);
            }
        }
    }
    void showPath()
    {
        //if (Path.Count > 5) { Debug.Log("Out Of Range!"); return; }
        for (int i = 0; i < Path.Count; i++)
        {
            Debug.Log(i + "��° ���: " + Path[i].x + ", " + Path[i].y);
        }
    }
}