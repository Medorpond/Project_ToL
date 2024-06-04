using System.Collections;
using System.Collections.Generic;
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
        moveRange = 10;
        coolTime1 = 3;
        coolTime2 = 7;
        weaponType = WeaponType.LightSword;
    }
    public override void Ability1()
    {
        base.Ability1();
        moveRange += 3;
        Debug.Log("moveRange +3");
    }

    public override void Ability2()
    {
        base.Ability2();
        maxHealth += 3;
        currentHealth += 3;
        Debug.Log("maxHealth +3, currentHealth +3");
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