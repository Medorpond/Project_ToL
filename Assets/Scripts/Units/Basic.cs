using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Basic : Unit
{
    protected override void Init()
    {
        maxHealth = 6;
        attackDamage = 3;
        attackRange = 3;
        moveRange = 3;
        skill_1_Cooldown= 5;
        skill_2_Cooldown= 7;
        base.Init();
        weaponType = WeaponType.LightSword;

    }

    public override bool Ability1()
    {
        if (skill_1_currentCool > 0) return false;
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.moveRange += 2;
        };

        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.moveRange -= 2;
        };

        Buff moveFar = new Buff(1, onApply, null, onRemove, this);
        moveFar.Apply();
        skill_1_currentCool = skill_1_Cooldown;
        return true;
    }
    public override bool Ability2()
    {
        if (skill_2_currentCool > 0) return false;
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.attackRange += 1;
        };

        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.attackRange -= 1;
        };

        Buff attackFar = new Buff(1, onApply, null, onRemove, this);
        attackFar.Apply();
        skill_2_currentCool = skill_2_Cooldown;
        return true;
    }
}
