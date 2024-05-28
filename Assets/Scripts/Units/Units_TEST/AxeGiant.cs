using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class AxeGiant : Unit
{
    private List<GameObject> myUnits;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 6;
        currentHealth = maxHealth;
        attackDamage = 4;
        attackRange = 1;
        moveRange = 3;
        coolTime1 = 6;
        coolTime2 = 8;

        myUnits = GetComponentInParent<PlayerManager>().UnitList;
    }
    public override void Ability1()
    {
        base.Ability1();
        moveRange += 2;
    }

    public override void Ability2()
    {
        base.Ability2();
        Collider[] colliders = Physics.OverlapSphere(transform.position, moveRange);
        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                if (!CheckTeam(unit)) unit.IsDamaged(2.5f);
            }
        }
    }
    protected override void AfterAbility1()
    {
        moveRange -= 2;
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        skillActive2 = false;
    }

    private bool CheckTeam(Unit unit)
    {
        foreach (GameObject myUnit in myUnits)
        {
            if (myUnit == unit) return true;
        }
        return false;
    }
}
