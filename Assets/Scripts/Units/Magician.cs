using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Magician : Unit
{
    private List<GameObject> myUnits;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 3;
        coolTime1 = 7;
        coolTime2 = 4;

        myUnits = GetUnitList();
    }

    public override bool Ability1()
    {
        base.Ability1();

        myUnits = GetUnitList();

        myUnits.Remove(gameObject);

        Node[,] NodeArray = MapManager.Instance.stage.NodeArray;

        while (myUnits != null)
        {
            GameObject RandomUnit = myUnits[Random.Range(0, myUnits.Count)];

            int x = (int)RandomUnit.transform.position.x;
            int y = (int)RandomUnit.transform.position.y;

            if (CheckNode(x, y + 1)) return true;
            if (CheckNode(x, y - 1)) return true;
            if (CheckNode(x + 1, y)) return true;
            if (CheckNode(x - 1, y)) return true;
            else myUnits.Remove(RandomUnit);
        }

        if (myUnits == null) return false;

        return false;

        bool CheckNode(int x, int y)
        {
            if (!NodeArray[x, y].isBlocked && NodeArray[x, y].isDeployable)
            {
                transform.position = new Vector3(NodeArray[x, y].x, NodeArray[x, y].y, transform.position.z);

                return true;
            }
            else return false;
        }
    }

    public override bool Ability2()
    {
        base.Ability2();
        attackRange = float.MaxValue;
        return true;
    }
    protected override void AfterAbility1()
    {
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        attackRange = 1;
        skillActive2 = false;
    }
}
