using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public class Captain : Unit
{
    private List<GameObject> myUnits;
    private float increaseAttack;
    public Vector3 skillDirection;
    private int skillRange;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 9;
        currentHealth = maxHealth;
        attackDamage = 1;
        attackRange = 1;
        moveRange = 30;
        coolTime1 = 4;
        coolTime2 = 5;
        weaponType = WeaponType.LightSword;

        GetUnitList();
        increaseAttack = 1.0f;
        skillRange = (int)(moveRange * 1.5f);
    }

    public override void IsDead()
    {
        // Trigger Death Animation
        MatchManager.Instance.GameOver();
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
            if (direction.x == 0 && direction.y == 0) return false;
            else
            {
                if (direction.x == 0 || direction.y == 0) return true;
                if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y)) return true;
            }
            return false;
        }

        bool CanMove()
        {
            Node[,] NodeArray = MapManager.Instance.stage.NodeArray;

            int length = (int)Vector3.Magnitude(direction);
            if (length > skillRange) return false;

            direction = Vector3.Normalize(direction);
            if (direction.x != 0 || direction.y != 0) direction = new Vector3(1, 1, 0);

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
        GetUnitList();

        if (myUnits != null)
        {
            foreach (GameObject unit in myUnits)
            {
                unit.GetComponent<Unit>().ChangeAttackDamage(increaseAttack);
            }
        }
    }
    protected override void AfterAbility1()
    {
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        if (currentCool2 == 3)
        {
            GetUnitList();
            if (myUnits != null)
            {
                foreach (GameObject unit in myUnits)
                {
                    unit.GetComponent<Unit>().ChangeAttackDamage(-increaseAttack);
                }
            }
            skillActive2 = false;
        }
    }

    public void Passive()
    {
        currentHealth = maxHealth;
        moveRange += 7;
        attackDamage += 4;
    }

    private void GetUnitList()
    {
        myUnits = GetComponentInParent<PlayerManager>().UnitList;
    }
}
