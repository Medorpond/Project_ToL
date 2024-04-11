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
            restrictBottom = _NodeArray[0, 0];
            restrictTop = _NodeArray[15, 15];

            StartNode.D = 0;

            NodeArray = _NodeArray;

            OpenList = new List<Node>() { StartNode };
            ClosedList = new List<Node>();
            Path = new List<Node>();
        }
        #region Parameter
        Node[,] NodeArray;
        Node StartNode, TargetNode, CurrentNode;
        Node restrictBottom, restrictTop;
        int movePoint;
        int scale;
        List<Node> OpenList, ClosedList;

        public List<Node> Path;
        #endregion

        public void PathFinding()
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

                Scan(CurrentNode.x, CurrentNode.y + 10); // 상
                Scan(CurrentNode.x, CurrentNode.y - 10); // 하
                Scan(CurrentNode.x - 10, CurrentNode.y); // 좌
                Scan(CurrentNode.x + 10, CurrentNode.y); // 우
                Scan(CurrentNode.x + 10, CurrentNode.y + 10); // 대각 우상
                Scan(CurrentNode.x - 10, CurrentNode.y + 10); // 대각 좌상
                Scan(CurrentNode.x + 10, CurrentNode.y - 10); // 대각 우하
                Scan(CurrentNode.x - 10, CurrentNode.y - 10); // 대각 좌하
            }
        }

        void Scan(int _scanX, int _scanY)
        {
            if (_scanX <= 0 || _scanY <= 0)
            {
                return;
            }
            Node OnScanNode = NodeArray[_scanX/10, _scanY/10];

            if (OnScanNode.isBlocked || ClosedList.Contains(OnScanNode))
            { return; }// 탐색 대상이 벽이거나 이미 경로에 있으면 중단

            if (NodeArray[CurrentNode.x/10, _scanY / 10].isBlocked ||
                NodeArray[_scanX / 10, CurrentNode.y / 10].isBlocked)
            { return; }// 대각선 이동 중 코너를 가로지르게 되는 경우 중단

            if (_scanX < restrictBottom.x || _scanX > restrictTop.x ||
                _scanY < restrictBottom.y || _scanY > restrictTop.y)
            { return; }// 맵 경계 외부를 탐색할 경우 중단

            int moveCost = CurrentNode.D + (CurrentNode.x == _scanX || CurrentNode.y == _scanY ? 10 : 14);

            if (!OpenList.Contains(OnScanNode) || moveCost < OnScanNode.D)
            {
                OnScanNode.D = moveCost;
                OnScanNode.H = (Mathf.Abs(OnScanNode.x - TargetNode.x) + Mathf.Abs(OnScanNode.y - TargetNode.y));
                OnScanNode.ParantNode = CurrentNode;

                OpenList.Add(OnScanNode);
            }
        }

        public void OnDrawGizmos()
        {
            if (Path.Count != 0)
            {
                for (int i = 0; i < Path.Count; i++)
                {
                    Debug.Log("Now: " + Path[i].x + ", " + Path[i].y);
                }
            }
            else { Debug.Log("비었는데?"); }
        }
    }
}

