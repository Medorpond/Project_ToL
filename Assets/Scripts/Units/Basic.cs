using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : Unit
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
        attackDamage = 3;
        attackRange = 3;
        moveRange = 3;
        coolTime1 = 5;
        coolTime2 = 7;
        weaponType = WeaponType.LightSword;
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
        attackRange += 1;
        return true;
    }

    protected override void AfterAbility1()
    {
        moveRange -= 2;
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        attackRange -= 1;
        skillActive2 = false;
    }
}
