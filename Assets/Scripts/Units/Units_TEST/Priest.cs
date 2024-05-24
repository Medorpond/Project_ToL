using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        attackDamage = 0;
        attackRange = 0;
        moveRange = 4;
        coolTime1 = 3;
        coolTime2 = 5;

        myUnits = GetComponentInParent<PlayerManager>().UnitList;
        healAmount = 2.0f;
    }
    public override void Ability1()
    {
        base.Ability1();
        moveRange += 3;
    }

    public override void Ability2()
    {
        base.Ability2();
        
        foreach (GameObject unit in myUnits)
        {
            unit.GetComponent<Unit>().IsHealed(healAmount);
        }
    }
    protected override void AfterAbility1()
    {
        moveRange -= 3;
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        skillActive2 = false;
    }
}
