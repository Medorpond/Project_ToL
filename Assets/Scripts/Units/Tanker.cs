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
}
