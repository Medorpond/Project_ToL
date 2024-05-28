using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Magician : Unit
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
        attackRange = 1;
        moveRange = 3;
        coolTime1 = 7;
        coolTime2 = 4;
    }

    public override void Ability1()
    {
        base.Ability1();
    }

    public override void Ability2()
    {
        base.Ability2();
        attackRange = float.MaxValue;
    }
    protected override void AfterAbility1()
    {
        
    }
    protected override void AfterAbility2()
    {
        attackRange = 1;
        skillActive2 = false;
    }
}
