using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class Assassin : Unit
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 4;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 7;
        coolTime1 = 3;
        coolTime2 = 5;
        weaponType = WeaponType.DoubleBlade;

        originalAttackRange = attackRange;
        originalMoveRange = moveRange;
    }
    public override bool Ability1()
    {
        base.Ability1();

        if (canMove) return false;
        else
        {
            if (canAttack) return false;
            else
            {
                canMove = true;
                return true;
            }
        }
    }

    public override bool Ability2()
    {
        base.Ability2();
        ToxicDamage();
        return true;
    }
    protected override void AfterAbility1()
    {
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        if (currentCool2 == 4) ToxicDamage();
        if (currentCool2 == 3) skillActive2 = false;
    }

    protected void ToxicDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, moveRange * 2);

        if (colliders == null) return;

        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                unit.IsDamaged(1.5f);
            }
        }
    }
}
