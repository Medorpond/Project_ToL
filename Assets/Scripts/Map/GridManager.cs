using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Board Info")]
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [SerializeField]
    private MapTile tilePrefab;

    [SerializeField]
    private Camera camera;



    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 타일 생성
                MapTile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, gameObject.transform);
                spawnedTile.name = $"Tile({x}, {y})";

                // 타일의 타입 결정
                int randomNum = Random.Range(1, 10);    // 랜덤 요소가 몇개 생성되는지 랜덤으로(변경예정)
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) spawnedTile.tileType = TileType.block;
                else
                {
                    spawnedTile.tileType = TileType.grass;

                    // 랜덤으로 벽 생성
                    for (int i = 0; i < randomNum; i++)
                    {
                        if (x == Random.Range(1, width) && y == Random.Range(1, height)) spawnedTile.tileType = TileType.wall;
                    }
                }

                // 타입에 맞게 스프라이트 변경
                spawnedTile.SettingTile(spawnedTile.tileType);
            }
        }

        // 맵에 맞춰 카메라 조정
        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        camera.orthographicSize = height * 0.65f;
    }
}
