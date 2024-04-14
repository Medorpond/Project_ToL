using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
public class MapTileManager : MonoBehaviour
{
    // 32 X 17 Sized; 31 x 16
    private Node[,] NodeArray;

    public PathFinder pathfinder;

    private void Awake()
    {
        NodeArray = new Node[32, 17];
        NodeArrayInit();
        pathfinder = PathFinder.GetInstance();
    }
    void Start()
    {
        pathfinder.Init(NodeArray);
    }
    private void NodeArrayInit()
    {
        bool isWall;
        for(int x = 0; x < 32; x++)
        {
            for(int y = 0; y < 17; y++)
            {
                if (x == 0 || x == 31) { isWall = true; }
                else if (14 <= x && x <= 17 && 6 <= y && y <= 10) { isWall = true; }
                else if ((x == 7 || x == 8 || x == 23 || x == 24) && (5 <= y && y <= 11)) { isWall = true; }
                else { isWall = false; }
                NodeArray[x, y] = new Node(isWall, x, y);
            }
        }
    }
}
