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

    public override void getDamage(int atk)
    {
        int previousHP = health;
        health = health - atk > 0 ? health - atk : 0;
        onHPEvent.Invoke(previousHP, health);

        if (health <= 0)
        {
            health = 0;
        }
    }
}
