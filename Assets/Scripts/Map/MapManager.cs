using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public class MapManager : MonoBehaviour
{
    #region Parameter
    private string stageSize;
    private string stageName;

    public PathFinder pathfinder;
    #endregion


    private void Awake()
    {
        stageSize = "3x3"; //Predefined for test
        stageName = "Standard"; //Predefined for test
    }
    private void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Map/" + stageSize + "/" +  stageName);
        if (prefab != null)
        {
            Instantiate(prefab, new Vector3(-0.5f, -0.5f), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Failed to load prefab from path: Prefabs/Map/" + stageSize + "/" + stageName);
        }

    }

}
