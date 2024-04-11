using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : Character
{
    // Tanker Ability : Get less damage
    private float decreaseDamage = 0.8f;



    public override void MoveTo()
    {
        // Move
    }

    public override void Ability()
    {
        // Ability
    }

    public override void Init()
    {
        maxHealth = 150;
        attackDamage = 7;

        base.Init();
    }

    public override bool DecreaseHP(int damage)
    {
        damage = (int)(damage * decreaseDamage);
        return base.DecreaseHP(damage);
    }
}
