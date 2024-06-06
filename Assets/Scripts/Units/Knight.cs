using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Knight : Unit
{
    private float increaseAttack;
    

    protected override void Awake()
    {
        base.Awake();
        Init();    
    }

    protected override void Init()
    {
        maxHealth = 7;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 3;
        coolTime1 = 3;
        coolTime2 = 7;
        weaponType = WeaponType.LightSword;
    }
    public override bool Ability1()
    {
        base.Ability1();
        moveRange += 3;
        return true;
    }

    public override bool Ability2()
    {
        base.Ability2();
        maxHealth += 3;
        currentHealth += 3;
        return true;
    }
    protected override void AfterAbility1()
    {
        moveRange -= 3;
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        if (currentCool2 == 4)
        {
            maxHealth -= 3;
            currentHealth -= 3;
            skillActive2 = false;
        }
    }
}
