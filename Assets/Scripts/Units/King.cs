using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Character
{
    public override void Init()
    {
        maxHealth = 10;
        attackDamage = 4;
        attackRange = 3;
        moveRange = 5;

        base.Init();
    }

    public override void Ability()
    {
        // Ability
    }
}
