using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System;

public class Knight : Unit
{
    protected override void Init()
    {
        maxHealth = 7;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 2;
        skill_1_Cooldown= 3;
        skill_2_Cooldown = 7;
        type = "Tanker";
        base.Init();
        weaponType = WeaponType.LightSword;
    }

    public override bool Ability1()
    {
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.moveRange += 1;
        };

        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.moveRange -= 1;
        };

        Buff moveFar = new Buff(1, onApply, null, onRemove, this);
        moveFar.Apply();
        skill_1_currentCool = skill_1_Cooldown;
        return true;
    }

    public override bool Ability2()
    {
        Action<Unit> onApply = (Unit _unit) =>
        {
            _unit.maxHealth += 3;
            _unit.currentHealth += 3;
        };
        Action<Unit> onRemove = (Unit _unit) =>
        {
            _unit.maxHealth -= 3;
            if (_unit.currentHealth > _unit.maxHealth) _unit.currentHealth = _unit.maxHealth;
        };
        Buff IncreaseHealth = new Buff(3, onApply, null, onRemove, this);
        IncreaseHealth.Apply();
        skill_2_currentCool = skill_2_Cooldown;
        return true;
    }

    protected override void AnalizeAction() 
    {
        mostValuedAction = (1, "");
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 movePos = myPos;
        Unit TargetUnit = null;
        float closestDist = Mathf.Infinity;
        float distantDist = 0;
        bool inRange = false;

        int weight;
        string command = "";

        foreach (Node node in movableNode)
        {
            Vector2 psudoPos = new Vector2(node.x, node.y);
            foreach (Unit unit in opponent.UnitList)
            {
                float dist = Vector2.Distance(unit.transform.position, psudoPos);
                if (dist <= attackRange)
                {
                    inRange = true;
                    if (TargetUnit == null)
                    {
                        TargetUnit = unit;
                        distantDist = dist;
                        movePos = psudoPos;
                        break;
                    }
                    else if (TargetUnit == unit)
                    {
                        if (distantDist < dist)
                        {
                            distantDist = dist;
                            movePos = psudoPos;
                        }
                        break;
                    }
                    else if (unit.currentHealth <= attackDamage)
                    {
                        if (TargetUnit.currentHealth > attackDamage)
                        {
                            TargetUnit = unit;
                            distantDist = dist;
                            movePos = psudoPos;
                            break;
                        }
                        else
                        {
                            if (TargetUnit.type == "Captain")
                            {
                                break;
                            }
                            else if (unit.type == "Captain")
                            {
                                TargetUnit = unit;
                                distantDist = dist;
                                movePos = psudoPos;
                                break;
                            }
                            else if (TargetUnit.type == unit.type)
                            {
                                if (TargetUnit.currentHealth < unit.currentHealth)
                                {
                                    TargetUnit = unit;
                                    distantDist = dist;
                                    movePos = psudoPos;
                                }
                                break;
                            }
                            else
                            {
                                if (unit.type == "Healer")
                                {
                                    TargetUnit = unit;
                                    distantDist = dist;
                                    movePos = psudoPos;
                                    break;
                                }
                                else if (unit.type == "Dealer" && TargetUnit.type != "Healer")
                                {
                                    TargetUnit = unit;
                                    distantDist = dist;
                                    movePos = psudoPos;
                                    break;
                                }
                                else break;
                            }
                        }


                    }
                    else
                    {
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            TargetUnit = unit;
                        }
                    }
                }
                else
                {
                    if (dist < closestDist && !inRange)
                    {
                        movePos = psudoPos;
                        closestDist = dist;
                    }
                }
            }
        }

        



        if (myPos != movePos) command += $"@Move/({myPos.x},{myPos.y})/({movePos.x},{movePos.y})";
        if (TargetUnit != null)
        {
            switch (TargetUnit.type)
            {
                case "Captain":
                    weight = 18;
                    break;
                case "Healer":
                    weight = 13;
                    break;
                case "Dealer":
                    weight = 10;
                    break;
                case "Tanker":
                    weight = 8;
                    break;
                default:
                    weight = 1;
                    break;
            }
            if (inRange)
            {
                weight += 5;
                command += $"@Attack/({movePos.x},{movePos.y})/({TargetUnit.transform.position.x},{TargetUnit.transform.position.y})";
            }
        }
        else weight = 1;

        command += $"@Idle/({movePos.x},{movePos.y})/({movePos.x},{movePos.y})";

        mostValuedAction = (weight, command);
    }
}
