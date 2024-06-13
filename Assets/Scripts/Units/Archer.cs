using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{

    protected override void Init()
    {
        maxHealth = 3;
        attackDamage = 2;
        attackRange = 5;
        moveRange = 4;
        skill_1_Cooldown= 3;
        skill_2_Cooldown = 3;
        type = "Dealer";
        base.Init();
        weaponType = WeaponType.ArrowAtk;
    }

    public override bool Ability1(GameObject destObj)
    {
        if (skill_1_currentCool > 0) return false;
        if (inAction == true) return false;
        
        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector3 targetPos_3 = destObj.transform.position;
        Vector2Int targetPos = 
            new Vector2Int((int)destObj.transform.position.x, (int)destObj.transform.position.y);
        Vector2Int direction = targetPos - startPos;

        if (CheckDirection())
        {
            inAction = true;
            MapManager.Instance.stage.Occupy(startPos, targetPos, this);
            
            TriggerMoveAnimation();
            StartCoroutine(MoveCoroutine());
            skill_1_currentCool = skill_1_Cooldown;
            return true;
        }
        else { Debug.Log("WrongDirection"); }

        IEnumerator MoveCoroutine()
        {
            while (Vector3.Distance(transform.position, targetPos_3) > 0.01f)
            { transform.position = Vector3.MoveTowards(transform.position, targetPos_3, moveSpeed); yield return null;}
            transform.position = targetPos_3;
            ResetMoveAnimation();
            inAction = false;
            ScanMovableNode();
        }
        return false;

        bool CheckDirection()
        {
            if (direction.x == 0 || direction.y == 0)
            {
                if (direction.x == 0 && direction.y == 0) return false;
                else return true;
            }
            return false;
        }
    }
    public override bool Ability2()
    {
        if (skill_2_currentCool > 0) return false;
        moveLeft++;
        attackLeft++;
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

        int weight;
        string command = "";
        
        

        if (myPos != movePos) command += $"@Move/({myPos.x},{myPos.y})/({movePos.x},{movePos.y})";
        if (TargetUnit != null)
        {
            switch (TargetUnit.type)
            {
                case "Captain":
                    weight = 20;
                    break;
                case "Healer":
                    weight = 15;
                    break;
                case "Dealer":
                    weight = 12;
                    break;
                case "Tanker":
                    weight = 10;
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
