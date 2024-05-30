using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Unit
{
    private List<GameObject> myUnits;
    private float increaseAttack;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 9;
        currentHealth = maxHealth;
        attackDamage = 1;
        attackRange = 1;
        moveRange = 30;
        coolTime1 = 4;
        coolTime2 = 5;
        weaponType = WeaponType.LightSword;

        GetUnitList();
        increaseAttack = 1.0f;
    }

    public override void IsDead()
    {
        // Trigger Death Animation
        MatchManager.Instance.GameOver();
    }

    public override void Ability1()
    {
        base.Ability1();
    }

    public override void Ability2()
    {
        base.Ability2();
        GetUnitList();

        if (myUnits != null)
        {
            foreach (GameObject unit in myUnits)
            {
                unit.GetComponent<Unit>().ChangeAttackDamage(increaseAttack);
            }
        }
    }
    protected override void AfterAbility1()
    {
        skillActive1 = false;
    }
    protected override void AfterAbility2()
    {
        if (currentCool2 == 3)
        {
            GetUnitList();
            if (myUnits != null)
            {
                foreach (GameObject unit in myUnits)
                {
                    unit.GetComponent<Unit>().ChangeAttackDamage(-increaseAttack);
                }
            }
            skillActive2 = false;
        }
    }

    public void Passive()
    {
        currentHealth = maxHealth;
        moveRange += 7;
        attackDamage += 4;
    }

    private bool CheckDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y)) return true;
        if (direction.x == 0 || direction.y == 0) return true;
        else return false;
    }

    private void GetUnitList()
    {
        myUnits = GetComponentInParent<PlayerManager>().UnitList;
    }
}
