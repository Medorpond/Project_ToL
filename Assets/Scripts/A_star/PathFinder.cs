using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Astar
{
    public class PathFinder
    {
        // scale은 10 (14 for Diagnal)으로 임의설정됨. 빠른 연산을 위해 int형 scale 필요.
        public PathFinder(Node[,] _NodeArray, Vector2Int _startpoint, Vector2Int _destination,
            int _movePoint)
        {
            movePoint = _movePoint;

            StartNode = _NodeArray[_startpoint.x, _startpoint.y];
            TargetNode = _NodeArray[_destination.x, _destination.y];
            restrictBottom = new Vector2(_NodeArray[0, 0].x, _NodeArray[0, 0].y); // Start of Map, BottomLeft
            restrictTop = new Vector2(_NodeArray[15, 15].x, _NodeArray[15, 15].y); // End of Map, TopRight

            StartNode.D = 0;

            NodeArray = _NodeArray;

            OpenList = new List<Node>() { StartNode };
            ClosedList = new List<Node>();
            Path = new List<Node>();
        }
        #region Parameter
        Node[,] NodeArray;
        Node StartNode, TargetNode, CurrentNode;
        Vector2 restrictBottom, restrictTop;
        int movePoint;
        List<Node> OpenList, ClosedList;

        public List<Node> Path;
        #endregion

        public List<Node> PathFinding()
        {
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
                Debug.Log("실행!");
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

            if (NodeArray[CurrentNode.x, _scanY].isBlocked ||
                NodeArray[_scanX, CurrentNode.y].isBlocked)
            { return; }// 대각선 이동 중 코너를 가로지르게 되는 경우 중단



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
}

