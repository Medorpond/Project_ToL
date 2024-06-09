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
        moveRange = 10;
        skill_1_Cooldown= 3;
        skill_2_Cooldown = 3;
        base.Init();
        weaponType = WeaponType.ArrowAtk;
    }

    public override bool Ability1(GameObject destObj)
    {
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
        moveLeft++;
        attackLeft++;
        return true;
    }
}
