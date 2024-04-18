using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : Character
{
    // Tanker Ability : Get less damage
    private float decreaseDamage = 0.8f;



    public override void Init()
    {
        maxHealth = 7;
        attackDamage = 2;
        attackRange = 1;
        moveRange = 1;

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
    public override void Ability()
    {
        // Ability
    }

    public override bool DecreaseHP(int damage)
    {
        damage = (int)(damage * decreaseDamage);
        return base.DecreaseHP(damage);
    }
}
