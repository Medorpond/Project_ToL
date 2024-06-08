using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Unit
{
    private List<Unit> myUnits = new();
    public Vector3 skillDirection;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 9;
        attackDamage = 1;
        attackRange = 1;
        moveRange = 30;

        maxMoveCount = 99;

        skill_1_Cooldown = 4;
        skill_2_Cooldown= 5;
        base.Init();
        weaponType = WeaponType.LightSword;
    }

    public override void IsDead()
    {
        BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.deadSound);
        MatchManager.Instance.GameOver(transform.parent.gameObject);
    }

    public override bool Ability1()
    {
        base.Ability1();

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)skillDirection.x, (int)skillDirection.y);
        Vector2Int direction = targetPos - startPos;

        if (CheckDirection())
        {
            MapManager.Instance.stage.Occupy(startPos, targetPos, this);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, 0), moveSpeed);
            return true;
        }
        return false;

        bool CheckDirection()
        {
            if (direction.x == 0 && direction.y == 0) return false;
            else
            {
                if (direction.x == 0 || direction.y == 0) return true;
                if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y)) return true;
            }
            return false;
        }
    }

    public override bool Ability2()
    {
        myUnits = GetComponentInParent<PlayerManager>().UnitList;

        if (myUnits != null)
        {
            foreach (Unit unit in myUnits)
            {
                //Increase Damage via Buff
            }
            return true;
        }
        return false;
    }
    
    public void Rage()
    {
        currentHealth = maxHealth;
        moveRange += 7;
        attackDamage += 4;
    }
}
