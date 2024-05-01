using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public abstract class Stage : MonoBehaviour
{
    protected string stageType;
    protected int stageSize;

    protected Node[,] NodeArray; //PreDefine NodeArray for each Stage...

    public void Load()
    {

    }

    protected abstract void Init(); // Map Initialize itself by this Method.

    protected virtual void GenBuff() // 밸런스 문제로 보류
    {
        int numBuff; // Quantity of Buff or Debuff on Stage.
        switch (stageSize)
        {
            case 3:
                numBuff = 1;
                break;
            case 5:
                numBuff = 2;
                break;
            case 7:
                numBuff = 3;
                break;
            default:
                Debug.Log("Wrong MapSize");
                numBuff = 1;
                break;
        }// Set numBuff by stageSize.
        //Init Buffs under...
    }
}
