using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public class PathFinder
{
    #region Singletone
    private static PathFinder instance = null; // Singletone

    public static PathFinder GetInstance()
    {
        if (instance == null) { instance = new PathFinder(); }
        return instance;
    }

    public void Init(Node[,] _NodeArray)
    {
        NodeArray = _NodeArray;

        int scaleX = NodeArray.GetLength(0) - 1;
        int scaleY = NodeArray.GetLength(1) - 1;

        restrictBottom = new Vector2(NodeArray[0, 0].x, NodeArray[0, 0].y); // Start of Map, BottomLeft
        restrictTop = new Vector2(NodeArray[scaleX, scaleY].x, NodeArray[scaleX, scaleY].y); // End of Map, TopRight
    }

    public static PathFinder Instance { get { return instance; } }
    #endregion

    #region Parameter
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurrentNode;
    Vector2 restrictBottom, restrictTop;
    List<Node> OpenList, ClosedList;

    public List<Node> Path;
    #endregion

    public List<Node> PathFinding(Vector2Int _startPoint, Vector2Int _destination)
    {
        StartNode = NodeArray[_startPoint.x, _startPoint.y];
        TargetNode = NodeArray[_destination.x, _destination.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        Path = new List<Node>();

        while (OpenList.Count > 0)
        {

            CurrentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].W <= CurrentNode.W && OpenList[i].H < CurrentNode.H) { CurrentNode = OpenList[i]; }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                Node MileStone = TargetNode;
                while (MileStone != StartNode)
                {
                    Path.Add(MileStone);
                    MileStone = MileStone.ParantNode;
                }
                Path.Reverse();
            }

            Scan(CurrentNode.x, CurrentNode.y + 1); // 상
            Scan(CurrentNode.x, CurrentNode.y - 1); // 하
            Scan(CurrentNode.x - 1, CurrentNode.y); // 좌
            Scan(CurrentNode.x + 1, CurrentNode.y); // 우
            /*
            Scan(CurrentNode.x + 1, CurrentNode.y + 1); // 대각 우상
            Scan(CurrentNode.x - 1, CurrentNode.y + 1); // 대각 좌상
            Scan(CurrentNode.x + 1, CurrentNode.y - 1); // 대각 우하
            Scan(CurrentNode.x - 1, CurrentNode.y - 1); // 대각 좌하
            대각선 비활성... */
        }
        return Path;
    }

    void Scan(int _scanX, int _scanY)
    {
        if (_scanX < restrictBottom.x || _scanX > restrictTop.x ||
            _scanY < restrictBottom.y || _scanY > restrictTop.y)
        { return; }// 맵 경계 외부를 탐색할 경우 중단

        Node OnScanNode = NodeArray[_scanX, _scanY];

        if (OnScanNode.isBlocked || ClosedList.Contains(OnScanNode))
        { return; }// 탐색 대상이 벽이거나 이미 경로에 있으면 중단

        /*
        if (NodeArray[CurrentNode.x, _scanY].isBlocked ||
            NodeArray[_scanX, CurrentNode.y].isBlocked)
        { return; }// 대각선 이동 중 코너를 가로지르게 되는 경우 중단
        대각선 비활성 */



        int moveCost = CurrentNode.D + 1; // 대각선을 허용할 경우 => (CurrentNode.x == _scanX || CurrentNode.y == _scanY ? 10 : 14);

        if (!OpenList.Contains(OnScanNode) || moveCost < OnScanNode.D)
        {
            OnScanNode.D = moveCost;
            OnScanNode.H = (Mathf.Abs(OnScanNode.x - TargetNode.x) + Mathf.Abs(OnScanNode.y - TargetNode.y));
            OnScanNode.ParantNode = CurrentNode;

            OpenList.Add(OnScanNode);
        }
    }
}


