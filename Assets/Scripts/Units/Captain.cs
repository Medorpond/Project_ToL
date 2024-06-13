using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Unit
{
    private List<Unit> myUnits = new();
    public Vector3 skillDirection;

    protected override void Init()
    {
        maxHealth = 9;
        attackDamage = 1;
        attackRange = 1;
        moveRange = 30;
        type = "Captain";

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
        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)skillDirection.x, (int)skillDirection.y);
        Vector2Int direction = targetPos - startPos;

        if (CheckDirection())
        {
            MapManager.Instance.stage.Occupy(startPos, targetPos, this);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, 0), moveSpeed);
            skill_1_currentCool = skill_1_Cooldown;
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
                Action<Unit> onApply = (Unit _unit) =>
                {
                    _unit.attackDamage += 1;
                };

                Action<Unit> onRemove = (Unit _unit) =>
                {
                    _unit.attackDamage -= 1;
                };

                Buff increaseAttack = new Buff(3, onApply, null, onRemove, unit);
                increaseAttack.Apply();
            }
            skill_2_currentCool = skill_2_Cooldown;
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

    protected override void AnalizeAction() 
    {
        mostValuedAction = (0, "");
        Unit closestUnit = null;
        Vector3 myPos = this.transform.position;
        Vector3 movePos = myPos;
        foreach (Unit unit in opponent.UnitList)
        {
            if (closestUnit == null || Vector3.Distance(closestUnit.transform.position, myPos) 
                > Vector3.Distance(unit.transform.position, myPos))
            {
                closestUnit = unit;
            }
        }

        if (Vector3.Distance(closestUnit.transform.position, myPos) <= closestUnit.attackRange)
        {
            float distance = 0;
            Node distantNode = null;
            foreach (Node node in movableNode)
            {
                float curDis = Vector3.Distance(new Vector2(node.x, node.y), closestUnit.transform.position);
                if ( curDis > closestUnit.attackRange && curDis > distance)
                {
                    distance = curDis;
                    distantNode = node;
                }
            }
            if (distantNode != null)
            {
                mostValuedAction.weight = 30;
                mostValuedAction.command += $"@Move/({myPos.x},{myPos.y})/({distantNode.x},{distantNode.y})";
                movePos = new Vector3(distantNode.x, distantNode.y);
            }
        }

        mostValuedAction.command += $"@Idle/({movePos.x},{movePos.y})/({movePos.x},{movePos.y})";
    }
}
