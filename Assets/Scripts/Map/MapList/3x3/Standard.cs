using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public class Standard : Stage
{
    public override void Init()
    {
        stageType = "standard";
        stageSize = 3;
        NodeArray = new Node[27, 11];
        NodeInit();
    }
    void NodeInit()
    {
        bool isBlocked = false;
        for (int x = 0; x < 27; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                NodeArray[x, y] = new Node(isBlocked, x, y);
            }
        }
    }
}
