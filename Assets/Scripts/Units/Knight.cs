using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System;

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
        skill_1_Cooldown= 3;
        skill_2_Cooldown = 7;
        weaponType = WeaponType.LightSword;
    }

    public override bool Ability1()
    {
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.moveRange += 3;
        };

        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.moveRange -= 3;
        };

        Buff moveFar = new Buff(1, onApply, null, onRemove, this);
        moveFar.Apply();
        return true;
    }

    public override bool Ability2()
    {
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.maxHealth += 3;
            _unit.currentHealth += 3;
        };
        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.maxHealth -= 3;
        };
        Buff IncreaseHealth = new Buff(3, onApply, null, onRemove, this);
        IncreaseHealth.Apply();
        return true;
    }
}
