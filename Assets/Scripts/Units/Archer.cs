using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
using TMPro;

public class Archer : Unit
{
    public Vector3 skillDirection;
    private int skillRange;

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
        attackRange = 5;
        moveRange = 10;
        coolTime1 = 3;
        coolTime2 = 3;
        weaponType = WeaponType.ArrowAtk;
        skillRange = (int)(moveRange * 1.5f);
    }

    public override void Ability1()
    {
        base.Ability1();

        Vector3 direction = new Vector3(skillDirection.x - transform.position.x, skillDirection.y - transform.position.y, 0);
        if (CheckDirection())
        {
            if (CanMove())
            {
                transform.position = Vector3.MoveTowards(transform.position, skillDirection, moveSpeed);
            }
        }

        bool CheckDirection()
        {
            if (direction.x == 0 || direction.y == 0)
            {
                if (direction.x == 0 && direction.y == 0) return false;
                else return true;
            }
            return false;
        }

        bool CanMove()
        {
            Node[,] NodeArray = MapManager.Instance.stage.NodeArray;

            int length = (int)Vector3.Magnitude(direction);
            if (length > skillRange) return false;

            direction = Vector3.Normalize(direction);

            for (int i = 1; i <= length; i++)
            {
                if (NodeArray[(int)(transform.position.x + direction.x * i), (int)(transform.position.y + direction.y * i)].isBlocked) return false;
            }

            return true;
        }
    }
    public override void Ability2()
    {
        base.Ability2();
        canAttack = true;
        canMove = true;
    }
    protected override void AfterAbility1()
    {
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        skillActive2 = false;
    }
}
