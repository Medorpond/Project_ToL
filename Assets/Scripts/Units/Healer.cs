using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character
{
    // Healer Ability : Healing
    private int healAmount = 2;



    public override void Init()
    {
        maxHealth = 4;
        attackDamage = 10;
        attackRange = 2;
        moveRange = 2;

        base.Init();
    }
    /*
    public override void MoveTo(int direction)
    {
        // temp direction ( up : 1, down : 2, right : 3, left : 4)
        if (direction == 1) location += new Vector3(0, moveRange, 0);
        if (direction == 2) location -= new Vector3(0, moveRange, 0);
        if (direction == 3) location += new Vector3(moveRange, 0, 0);
        if (direction == 4) location -= new Vector3(moveRange, 0, 0);
    }
    */
    public void Ability()
    {
        base.IncreaseHP(healAmount);
    }
}
