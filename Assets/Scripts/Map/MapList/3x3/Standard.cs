using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public class Standard : Stage
{
    public Standard()
    {
        stageType = "Basic";
        stageSize = 3;
        restrictBottom = new Vector3(0, 0);
        restrictTop = new Vector3(26, 10);
        NodeArray = new Node[27, 11]; // Dedicated NodeArray
        PlayerLeaderPosition = new Vector3(2, 5);
        OpponentLeaderPosition = new Vector3(24, 5);
        NodeInit();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void NodeInit()
    {
        bool isBlocked;
        bool isDeployable;
        for (int x = 0; x < 27; x++)
        {
            for (int y = 0; y < 11; y++) 
            { 
                if (x == 0 || x == 26) isBlocked = true; else isBlocked = false;
                if ((x >= 0 && x <= 6) || (x <= 26 && x >= 21)) isDeployable = true; else isDeployable = false;
                NodeArray[x, y] = new Node(isDeployable, isBlocked, x, y);
            }
        }
        NodeArray[2, 5].isDeployable = false;
        NodeArray[24, 5].isDeployable = false;
    }
}
