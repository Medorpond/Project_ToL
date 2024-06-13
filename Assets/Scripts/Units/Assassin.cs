using System.Collections.Generic;
using UnityEngine;
using System;

public class Assassin : Unit
{
    protected override void Init()
    {
        maxHealth = 4;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 7;
        skill_1_Cooldown = 3;
        skill_1_Cooldown = 5;
        base.Init();
        weaponType = WeaponType.DoubleBlade;
    }
    public override bool Ability1()
    {
        if (skill_1_currentCool > 0) return false;
        if (attackLeft > 0) return false;
        else
        {
            moveLeft ++;
            skill_1_currentCool = skill_1_Cooldown;
            return true;
        }
    }

    public override bool Ability2()
    {
        if (skill_2_currentCool > 0) return false;
        ToxicDamage();
        skill_2_currentCool = skill_2_Cooldown;
        return true;
    }

    protected void ToxicDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, moveRange * 2);
        if (colliders == null) return;

        List<Unit> units = new List<Unit>();
        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                units.Add(unit); // Unit 컴포넌트를 가진 객체만 리스트에 추가
            }
        }

        

        Action<Unit> onTurnEnd = (Unit _unit) =>
        {
            _unit.IsDamaged(1.5f);
        };

        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                Buff Intoxication = new Buff(2, null, onTurnEnd, null, unit);
                Intoxication.Apply();
            }
        }
    }
}
