using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character
{
    // Healer Ability : Healing
    private int healAmount = 25;



    public override void MoveTo()
    {
        // make Move
    }

    public override void Ability()
    {
        base.IncreaseHP(healAmount);
    }

    public override void Init()
    {
        maxHealth = 100;
        attackDamage = 10;

        base.Init();
    }
}
