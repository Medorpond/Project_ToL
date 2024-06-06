using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeGiant : Unit
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 6;
        currentHealth = maxHealth;
        attackDamage = 4;
        attackRange = 1;
        moveRange = 3;
        coolTime1 = 6;
        coolTime2 = 8;
        weaponType = WeaponType.HeavyAttack;
    }
    public override bool Ability1()
    {
        base.Ability1();
        moveRange += 2;
        return true;
    }

    public override bool Ability2()
    {
        base.Ability2();

        Collider[] colliders = Physics.OverlapSphere(transform.position, moveRange);
        
        if (colliders == null) return false;

        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                unit.IsDamaged(2.5f);
            }
        }

        return true;
    }
    protected override void AfterAbility1()
    {
        moveRange -= 2;
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        skillActive2 = false;
    }
}
