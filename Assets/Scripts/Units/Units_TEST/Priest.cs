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
        attackDamage = 1;
        attackRange = 1;
        moveRange = 4;
        coolTime1 = 3;
        coolTime2 = 5;
        weaponType = WeaponType.healingMagic;

        GetUnitList();
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
        //���� ��� <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        return base.Attack(_opponent);
        //���� ��� <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.lightSwordAtk); <<���� �ߺ� ���?
    }

    private void GetUnitList()
    {
        myUnits = GetComponentInParent<PlayerManager>().UnitList;
    }
}
