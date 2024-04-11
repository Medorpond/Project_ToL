using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Astar
{
    public class MapMakerForAStarTest : MonoBehaviour
    {
        [Header("Board Info")]
        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private MapTile tilePrefab;

        public Node[,] NodeArray;

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
                PathFinder pathfinder = new PathFinder(NodeArray, new Vector2Int(0, 0), new Vector2Int(3, 5), 5);
                pathfinder.PathFinding();
                pathfinder.OnDrawGizmos();
            }
        }

        private void GenerateGrid()
        {
            bool isBlocked;
            for (int x = 0; x < width * 10; x += 10)
            {
                for (int y = 0; y < height * 10; y += 10)
                {
                    // 타일 생성
                    MapTile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, gameObject.transform);
                    spawnedTile.name = $"Tile({x}, {y})";

                    if ((x == 10 && y == 20) ||
                        (x == 20 && y == 10) ||
                        (x == 20 && y == 20) ||
                        (x == 40 && y == 10) ||
                        (x == 40 && y == 20) ||
                        (x == 40 && y == 30) ||
                        (x == 40 && y == 40) ||
                        (x == 10 && y == 40) ||
                        (x == 20 && y == 40) ||
                        (x == 30 && y == 40) ||
                        (x == 40 && y == 40)) { isBlocked = true; }
                    else { isBlocked = false; }
                    // 노드 추가
                    NodeArray[x / 10, y / 10] = new Node(isBlocked, x, y);
                }
            }
        }
    }
}