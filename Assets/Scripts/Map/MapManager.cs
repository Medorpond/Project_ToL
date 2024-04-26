using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NodeStruct;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private int MapNum;
    [SerializeField]
    public GameObject prefap;

    void Start()
    {
        LoadMap(MapNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadMap(int _mapNum)
    {
        Map map = (Map)_mapNum;
        string mapName = map;
        prefap = Resources.Load<GameObject>("3x3/)
    }
}
