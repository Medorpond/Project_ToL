using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Magician : Unit
{

    protected override void Init()
    {
        maxHealth = 3;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 3;
        skill_1_Cooldown = 7;
        skill_2_Cooldown = 4;
        base.Init();
    }

    public override bool Ability1(Unit unit)
    {

        //myUnits.Remove(gameObject); << ??

        Node[,] NodeArray = MapManager.Instance.stage.NodeArray;

        int x = (int)unit.transform.position.x;
        int y = (int)unit.transform.position.y;

        return Transform(x, y);


        // Local Method
        bool Transform(int x, int y)
        {
            Vector3 targetPos = new();
            if (!NodeArray[x, y - 1].isBlocked) targetPos = new Vector3(x, y - 1);
            else if (!NodeArray[x, y + 1].isBlocked) targetPos = new Vector3(x, y + 1);
            else if (!NodeArray[x - 1, y].isBlocked) targetPos = new Vector3(x - 1, y);
            else if (!NodeArray[x + 1, y].isBlocked) targetPos = new Vector3(x + 1, y);
            else return false;
            transform.position = targetPos;
            return true;
        }
    }
}
