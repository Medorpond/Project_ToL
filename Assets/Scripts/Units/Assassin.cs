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

        if (attackLeft > 0) return false;
        else
        {
            moveLeft ++;
            return true;
        }
    }

    public override bool Ability2()
    {
        ToxicDamage();
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
                Buff Intoxication = new Buff(3, null, onTurnEnd, null, unit);
                Intoxication.Apply();
            }
        }
    }
}
