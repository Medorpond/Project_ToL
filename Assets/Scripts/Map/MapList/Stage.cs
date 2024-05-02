using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public abstract class Stage : MonoBehaviour
{
    protected string stageType;
    protected int stageSize;
    

    [SerializeField]
    public GameObject stagePrefab;

    protected Node[,] NodeArray; //PreDefine NodeArray for each Stage...

    public abstract void Init(); // Map Initialize itself by this Method.

}
