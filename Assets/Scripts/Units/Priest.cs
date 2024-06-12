using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Priest : Unit
{
    private List<Unit> myUnits;

    protected override void Init()
    {
        maxHealth = 4;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 3;
        skill_1_Cooldown = 3;
        skill_2_Cooldown = 5;
        type = "Healer";

        base.Init();
        weaponType = WeaponType.healingMagic;
    }

    public override bool Ability1()
    {
        if (skill_1_currentCool > 0) return false;
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.moveRange += 2;
        };
        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.moveRange -= 2;
        };
        Buff constantHeal = new Buff(1, onApply, null, onRemove, this);
        constantHeal.Apply();
        return true;
    }

    public override bool Ability2()
    {
        if (skill_2_currentCool > 0) return false;

        foreach (Unit unit in player.UnitList)
        {
            unit.IsHealed(attackDamage);
        }
        this.attackLeft = 0; // Can't use Normal Heal If priest use Ability2
        return true;
    }

    public override bool Attack(Unit target)
    {
        // Priest heal by attack.
        if (target == null || target.type == "Captain") return false;

        target.IsHealed(attackDamage);
        if (this.skill_2_currentCool == 0) this.skill_2_currentCool += 1; // Can't use A2 If priest use Normal Heal
        return true;
    }

    protected override void AnalizeAction()
    {
        mostValuedAction = (1, "");
        List<Unit> damagedUnits = new();
        Unit mostDamagedUnit = null;
        bool isMDUHealed = false;
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);

        foreach (Unit unit in player.UnitList)
        {
            if (unit.currentHealth < unit.maxHealth)
            {
                damagedUnits.Add(unit);
                if (mostDamagedUnit == null || mostDamagedUnit.currentHealth > unit.currentHealth)
                {
                    mostDamagedUnit = unit;
                }
            }
        }

        if (skill_2_currentCool == 0 && damagedUnits.Count >= player.UnitList.Count/2)
        {
            mostValuedAction.weight = 15;
            mostValuedAction.command += $"@Ability2/({myPos.x},{myPos.y})/({myPos.x},{myPos.y})";
        }//If more than half is damaged, use A2
        else if (attackLeft > 0)
        {
            if (mostDamagedUnit != null && mostDamagedUnit.currentHealth < maxHealth / 4f)
            {
                if (Vector2.Distance(mostDamagedUnit.transform.position, myPos) <= attackRange)
                {
                    mostValuedAction.weight += 10;
                    mostValuedAction.command += $"@Attack/({myPos.x},{myPos.y})/({mostDamagedUnit.transform.position.x},{mostDamagedUnit.transform.position.y})";
                    isMDUHealed = true;
                }
            }//If MDU is seriously damaged and In range, heal MDU
            else if (this.currentHealth < maxHealth / 4f)
            {
                mostValuedAction.weight += 10;
                mostValuedAction.command += $"@Attack/({myPos.x},{myPos.y})/({myPos.x},{myPos.y})";
            }//If itself is seriously damaged, heal itself.
            else if (mostDamagedUnit != null)
            {
                if (Vector2.Distance(new Vector2(mostDamagedUnit.transform.position.x, mostDamagedUnit.transform.position.y), this.transform.position) <= attackRange)
                {
                    mostValuedAction.weight += 10;
                    mostValuedAction.command += $"@Attack/({myPos.x},{myPos.y})/({mostDamagedUnit.transform.position.x},{mostDamagedUnit.transform.position.y})";
                    isMDUHealed = true;
                }
            }
        }
        
        if(mostDamagedUnit != null && !isMDUHealed && moveLeft > 0 && mostDamagedUnit.type != "Captain")
        {
            float closestDistance = Mathf.Infinity;
            Node closestNode = null;
            if (skill_1_Cooldown == 0)
            {
                mostValuedAction.command += $"@Ability1/({myPos.x},{myPos.y})/({myPos.x},{myPos.y})";
                moveRange += 2;
                ScanMovableNode();
                moveRange -= 2;
            }
            foreach (Node node in movableNode)
            {
                float distance = Vector2.Distance(new Vector3(node.x, node.y), mostDamagedUnit.transform.position);
                if (distance < closestDistance)
                {
                    closestNode = node;
                    closestDistance = distance;
                }
            }
            if (closestNode != null)
            {
                if(closestNode.x != myPos.x || closestNode.y != myPos.y)
                {
                    if (!isMDUHealed) mostValuedAction.weight += 5;
                    mostValuedAction.command += $"@Move/({myPos.x},{myPos.y})/({closestNode.x},{closestNode.y})";
                    myPos = new Vector2(closestNode.x, closestNode.y);
                }
                
            }

            if (closestDistance <= attackRange && attackLeft > 0)
            {
                mostValuedAction.weight += 10;
                mostValuedAction.command += $"@Attack/({myPos.x},{myPos.y})/({mostDamagedUnit.transform.position.x},{mostDamagedUnit.transform.position.y})";
                isMDUHealed = true;
            }
        }
        mostValuedAction.command += $"@Idle/({myPos.x},{myPos.y})/({myPos.x},{myPos.y})";

    }
}
