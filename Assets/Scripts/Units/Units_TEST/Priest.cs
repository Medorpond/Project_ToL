using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Priest : Unit
{
    private List<GameObject> myUnits;
    private float healAmount;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 4;
        currentHealth = maxHealth;
        attackDamage = 1;
        attackRange = 1;
        moveRange = 4;
        coolTime1 = 3;
        coolTime2 = 5;
        weaponType = WeaponType.healingMagic;

        GetUnitList();
        healAmount = 2.0f;
    }

    public override bool Ability1(GameObject target)
    {
        if (!target.CompareTag("Unit")) { return false; }

        Unit unit = target.GetComponent<Unit>();

        Action<Unit> onTurnEnd = (Unit _unit) =>
        {
            _unit.IsHealed(1);
            Debug.Log("constantHeal!");
        };
        Buff constantHeal = new Buff(3, null, onTurnEnd, null, unit);
        constantHeal.Apply();
        return true;
    }

    public override void Ability2()
    {
        base.Ability2();
        GetUnitList();

        if (myUnits != null)
        {
            foreach (GameObject unit in myUnits)
            {
                unit.GetComponent<Unit>().IsHealed(healAmount);
            }
        }
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
        // skillActive2 = false;
    }

    public override bool Attack(GameObject _opponent)
    {
        return base.Attack(_opponent);   
    }

    private void GetUnitList()
    {
        myUnits = GetComponentInParent<PlayerManager>().UnitList;
    }
}
