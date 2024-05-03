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

    private void Start()
    {
        foreach (Node elem in NodeArray) { if(!elem.isBlocked) LoadTile(elem.x, elem.y); }
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

    void LoadTile(int x, int y)
    {
        string path = $"Prefabs/Map/Tile";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null) { Instantiate(prefab, new Vector3(x, y), Quaternion.identity, gameObject.transform).name = $"Tile({x}, {y})"; }
        else { Debug.LogError("Failed to load prefab from path: " + path); }
    }
}
