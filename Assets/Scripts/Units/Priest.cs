using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Priest : Unit
{
    private List<Unit> myUnits;
    private float healAmount;

    protected override void Init()
    {
        maxHealth = 4;
        currentHealth = maxHealth;
        attackDamage = 1;
        attackRange = 1;
        moveRange = 4;
        skill_1_Cooldown = 3;
        skill_2_Cooldown= 5;

        base.Init();
        weaponType = WeaponType.healingMagic;

        healAmount = 2.0f;
    }

    public override bool Ability1(Unit target)
    {
        if (!target.CompareTag("Unit")) { return false; }


        Action<Unit> onTurnEnd = (Unit _unit) =>
        {
            _unit.IsHealed(1);
            Debug.Log("constantHeal!");
        };
        Buff constantHeal = new Buff(3, null, onTurnEnd, null, target);
        constantHeal.Apply();
        return true;
    }

    public override bool Ability2()
    {
        myUnits = GetComponentInParent<PlayerManager>().UnitList;

        if (myUnits != null)
        {
            foreach (Unit unit in myUnits)
            {
                unit.GetComponent<Unit>().IsHealed(healAmount);
            }
            return true;
        }
        return false;
    }

    public override bool Attack(Unit target)
    {
        if (target == null) return false;

        target.IsHealed(healAmount);

        return true;
    }

    private void AnalizeAction()
    {
        
    }
}
