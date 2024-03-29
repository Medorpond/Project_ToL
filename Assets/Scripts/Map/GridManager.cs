using System.Collections;
using System.Collections.Generic;
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
    private Transform camTransform;



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
                MapTile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, gameObject.transform);
                spawnedTile.name = $"Tile({x}, {y})";

                bool chooseColor = (x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0);
                spawnedTile.SetTileColor(chooseColor);
            }
        }

        camTransform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }
}
