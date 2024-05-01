using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Map : MonoBehaviour
{
    protected string symType;
    protected string mapType;
    protected int mapSize;
    

    public void Load()
    {

    }

    protected abstract void Init(); // Map Initialize itself by this Method.

    private void genBuff()
    {

    }
}
