using System.Collections;
using System.Collections.Generic;
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
        
    }
    protected override void AfterAbility2()
    {

    }
}
