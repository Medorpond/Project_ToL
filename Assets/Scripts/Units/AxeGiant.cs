using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeGiant : Unit
{
    protected override void Init()
    {
        maxHealth = 6;
        attackDamage = 4;
        attackRange = 1;
        moveRange = 3;
        skill_1_Cooldown= 6;
        skill_2_Cooldown= 8;

        base.Init();
        weaponType = WeaponType.HeavyAttack;
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, moveRange * 2);
        if (colliders == null) return false;

        List<Unit> units = new List<Unit>();
        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                units.Add(unit); // Unit 컴포넌트를 가진 객체만 리스트에 추가
            }
        }

        foreach (Unit unit in units)
        {
            if (unit != null)
            {
                unit.IsDamaged(2.5f);
            }
        }
        skill_2_currentCool = skill_2_Cooldown;
        return true;
    }
}
