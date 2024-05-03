using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public abstract class Stage : MonoBehaviour
{
    protected string stageType;
    protected int stageSize;

    public Node[,] NodeArray; //PreDefine NodeArray for each Stage...

    protected virtual void Start()
    {
        GameObject gridSet = new GameObject("GridSet");
        gridSet.transform.parent = transform;
        
        foreach (Node elem in NodeArray) { if (!elem.isBlocked) LoadTile(elem.x, elem.y, gridSet.transform); }
    }

    //public abstract void Init(); // Map Initialize itself by this Method.
    protected void LoadTile(int x, int y, Transform parent)
    {
        string path = $"Prefabs/Map/Tile";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null) { Instantiate(prefab, new Vector3(x, y), Quaternion.identity, parent).name = $"Tile({x}, {y})"; }
        else { Debug.LogError("Failed to load prefab from path: " + path); }
    }
}
