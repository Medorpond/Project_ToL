using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        myUnits = GetUnitList();
        increaseAttack = 1.0f;
        skillRange = (int)(moveRange * 1.5f);
    }

    public override void IsDead()
    {
        // Trigger Death Animation
        MatchManager.Instance.GameOver();
    }

    public override bool Ability1()
    {
        base.Ability1();

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)skillDirection.x, (int)skillDirection.y);
        Vector2Int direction = targetPos - startPos;

        if (CheckDirection())
        {
            MapManager.Instance.stage.Occupy(startPos, targetPos, gameObject);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, 0), moveSpeed);
            return true;
        }
        return false;

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
        /*
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
        */
    }

    public override bool Ability2()
    {
        base.Ability2();
        myUnits = GetUnitList();

        if (myUnits != null)
        {
            foreach (GameObject unit in myUnits)
            {
                unit.GetComponent<Unit>().ChangeAttackDamage(increaseAttack);
            }
            return true;
        }
        return false;
    }
    protected override void AfterAbility1()
    {
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        if (currentCool2 == 3)
        {
            myUnits = GetUnitList();
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

    public override bool CheckAbilityMove(Vector3 direction)
    {
        if (skillActive1)
        {
            skillDirection = direction;
            return true;
        }
        return base.CheckAbilityMove(direction);
    }
}
