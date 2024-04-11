using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Astar
{
    public class PathFinder
    {
        // scale�� 10 (14 for Diagnal)���� ���Ǽ�����. ���� ������ ���� int�� scale �ʿ�.
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

                Scan(CurrentNode.x, CurrentNode.y + 1); // ��
                Scan(CurrentNode.x, CurrentNode.y - 1); // ��
                Scan(CurrentNode.x - 1, CurrentNode.y); // ��
                Scan(CurrentNode.x + 1, CurrentNode.y); // ��
                /*
                Scan(CurrentNode.x + 1, CurrentNode.y + 1); // �밢 ���
                Scan(CurrentNode.x - 1, CurrentNode.y + 1); // �밢 �»�
                Scan(CurrentNode.x + 1, CurrentNode.y - 1); // �밢 ����
                Scan(CurrentNode.x - 1, CurrentNode.y - 1); // �밢 ����
                �밢�� ��Ȱ��... */
                Debug.Log("����!");
            }
            return Path;
        }

        void Scan(int _scanX, int _scanY)
        {
            if (_scanX < restrictBottom.x || _scanX > restrictTop.x ||
                _scanY < restrictBottom.y || _scanY > restrictTop.y)
            { return; }// �� ��� �ܺθ� Ž���� ��� �ߴ�

            Node OnScanNode = NodeArray[_scanX, _scanY];

            if (OnScanNode.isBlocked || ClosedList.Contains(OnScanNode))
            { return; }// Ž�� ����� ���̰ų� �̹� ��ο� ������ �ߴ�

            if (NodeArray[CurrentNode.x, _scanY].isBlocked ||
                NodeArray[_scanX, CurrentNode.y].isBlocked)
            { return; }// �밢�� �̵� �� �ڳʸ� ���������� �Ǵ� ��� �ߴ�



            int moveCost = CurrentNode.D + 1; // �밢���� ����� ��� => (CurrentNode.x == _scanX || CurrentNode.y == _scanY ? 10 : 14);

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

