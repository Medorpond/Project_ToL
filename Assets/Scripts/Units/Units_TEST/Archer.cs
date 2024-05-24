using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
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
        moveRange = 7;
        coolTime1 = 3;
        coolTime2 = 3;
    }

    public override void MoveTo(Vector3 direction)
    {
        if (skillActive1)
        {
            if (CheckDirection(direction))
            {
                moveRange *= 2;
                base.MoveTo(direction);
            }
        }
        else base.MoveTo(direction);
    }

    public override void Ability1()
    {
        base.Ability1();
    }

    public override void Ability2()
    {
        base.Ability2();
    }
    protected override void AfterAbility1()
    {
        moveRange = 7;
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {

    }

    private bool CheckDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y)) return true;
        else return false;
    }
}
