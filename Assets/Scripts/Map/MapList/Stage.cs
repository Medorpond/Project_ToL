using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
using UnityEngine.Rendering;

public abstract class Stage : MonoBehaviour
{
    protected string stageType;
    protected int stageSize; // Num Of Units can placed
    public Vector3 PlayerLeaderPosition;
    public Vector3 OpponentLeaderPosition;


public Node[,] NodeArray; //PreDefine NodeArray for each Stage...
    protected Vector3 restrictBottom;
    protected Vector3 restrictTop;

    protected virtual void Start()
    {
        GameObject gridSet = new GameObject("GridSet");
        gridSet.transform.parent = transform;

        
        foreach (Node elem in NodeArray) { if (!elem.isBlocked) LoadTile(elem.x, elem.y, gridSet.transform); }
    }

    protected void LoadTile(int x, int y, Transform parent)
    {
        string path = $"Prefabs/Map/Tile";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null) { Instantiate(prefab, new Vector3(x, y, 1), Quaternion.identity, parent).name = $"Tile({x}, {y})"; }
        else { Debug.LogError("Failed to load prefab from path: " + path); }
    }

    protected abstract void NodeInit();

    public List<Node> Pathfinding(Vector2Int startPos, Vector2Int targetPos)
    {
        Node[,] Field = NodeArray;

        Node StartNode = Field[startPos.x, startPos.y];
        Node TargetNode = Field[targetPos.x, targetPos.y];
        Node CurrentNode;

        List<Node> OpenList = new List<Node>() { StartNode };
        List<Node> ClosedList = new List<Node>();
        List<Node> Path = new List<Node>();

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].W <= CurrentNode.W && OpenList[i].H < CurrentNode.H)
                { CurrentNode = OpenList[i]; }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                Node MileStone = TargetNode;
                while (MileStone != StartNode)
                {
                    Path.Add(MileStone);
                    MileStone = MileStone.ParentNode;
                }
                Path.Reverse();
                OpenList.Clear();
                continue;
            }

            Scan(CurrentNode.x, CurrentNode.y + 1);
            Scan(CurrentNode.x, CurrentNode.y - 1);
            Scan(CurrentNode.x - 1, CurrentNode.y);
            Scan(CurrentNode.x + 1, CurrentNode.y);
        }
        return Path;

        void Scan(int scanX, int scanY)
        {
            if (scanX < restrictBottom.x || scanX > restrictTop.x 
                ||scanY < restrictBottom.y || scanY > restrictTop.y)
            { return; }// 맵 경계 외부를 탐색할 경우 중단

            Node ScanNode = Field[scanX, scanY];

            if(ScanNode.isBlocked || ClosedList.Contains(ScanNode)) { return; }

            int moveCost = CurrentNode.D + 1;

            if( moveCost < ScanNode.D || !OpenList.Contains(ScanNode) )
            {
                ScanNode.D = moveCost;
                ScanNode.H
                    = (Mathf.Abs(ScanNode.x - TargetNode.x) + Mathf.Abs(ScanNode.y - TargetNode.y));
                ScanNode.ParentNode = CurrentNode;

                OpenList.Add(ScanNode);
            }
        }
    }


    public void Occupy(Vector2Int startPos, Vector2Int targetPos)
    {
        NodeArray[startPos.x, startPos.y].isBlocked = false;
        NodeArray[targetPos.x, targetPos.y].isBlocked = true;
    }
}
