using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : Character
{
    // Dealer Ability



    public override void Init()
    {
        maxHealth = 6;
        attackDamage = 4;
        attackRange = 3;
        moveRange = 3;

        base.Init();
    }


}
