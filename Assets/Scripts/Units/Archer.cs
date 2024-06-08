using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public Vector3 skillDirection;

    protected override void Init()
    {
        maxHealth = 3;
        attackDamage = 2;
        attackRange = 5;
        moveRange = 10;
        skill_1_Cooldown= 3;
        skill_2_Cooldown = 3;
        base.Init();
        weaponType = WeaponType.ArrowAtk;
    }

    public override bool Ability1()
    {
        base.Ability1();

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)skillDirection.x, (int)skillDirection.y);
        Vector2Int direction = targetPos - startPos;

        if (CheckDirection())
        {
            MapManager.Instance.stage.Occupy(startPos, targetPos, this);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, 0), moveSpeed);
            return true;
        }
        return false;

        bool CheckDirection()
        {
            if (direction.x == 0 || direction.y == 0)
            {
                if (direction.x == 0 && direction.y == 0) return false;
                else return true;
            }
            return false;
        }
    }
    public override bool Ability2()
    {
        moveLeft++;
        attackLeft++;
        return true;
    }
    

    public bool CheckAbilityMove(Vector3 direction)
    {
        if (skill_1_currentCool <= 0)
        {
            skillDirection = direction;
            return true;
        }
        else return false;
    }
}
