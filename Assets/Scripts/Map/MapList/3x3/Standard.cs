using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
using System.Security.Cryptography.X509Certificates;

public class Standard : Stage
{
    public Standard()
    {
        stageType = "Basic";
        stageSize = 3;
        NodeArray = new Node[27, 11];
        NodeInit();
    }
    protected override void Start()
    {
        base.Start();
    }
    void NodeInit()
    {
        bool isBlocked;
        for (int x = 0; x < 27; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                if (x == 0 || x == 26) isBlocked = true; else isBlocked = false;
                NodeArray[x, y] = new Node(isBlocked, x, y);
            }
        }
    }

    
}
