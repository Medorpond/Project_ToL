using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using NodeStruct;
using JetBrains.Annotations;

public class GridManager : MonoBehaviour
{
    [Header("Board Info")]
    public int width;
    public int height;

    [SerializeField]
    private MapTile tilePrefab;

    [SerializeField]
    private Camera camera;

    private NodeStruct.Node[,] NodeArray;
    public PathFinder pathfinder;


    private void Start()
    {
        NodeArray = new NodeStruct.Node[width, height];
        GenerateGrid();
        pathfinder = PathFinder.GetInstance();
        //pathfinder.Init(NodeArray);
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isBlocked = false;
                // Ÿ�� ����
                MapTile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, gameObject.transform);
                spawnedTile.name = $"Tile({x}, {y})";

                // Ÿ���� Ÿ�� ����
                int randomNum = Random.Range(1, 10);    // ���� ��Ұ� � �����Ǵ��� ��������(���濹��)

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) 
                { 
                    spawnedTile.tileType = TileType.block; 
                    isBlocked = true; 
                }
                else
                {
                    spawnedTile.tileType = TileType.grass;

                    // �������� �� ����
                    for (int i = 0; i < randomNum; i++)
                    {
                        if (x == Random.Range(1, width) && y == Random.Range(1, height)) spawnedTile.tileType = TileType.wall;
                    }
                }

                // Ÿ�Կ� �°� ��� �ʱ�ȭ
                NodeArray[x, y] = new NodeStruct.Node(isBlocked , x, y);

                // Ÿ�Կ� �°� ��������Ʈ ����
                spawnedTile.SettingTile(spawnedTile.tileType);
            }
        }

        // �ʿ� ���� ī�޶� ����
        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        camera.orthographicSize = height * 0.65f;
    }
}
