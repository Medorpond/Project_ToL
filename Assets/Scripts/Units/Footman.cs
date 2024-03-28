using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootMan : Unit
{
    protected override void Start()
    {
        baseHP = 100;
        baseMovePoint = 5f;
        baseMoveSpeed = 5f;
        baseDamage = 25;
        baseRange = 1;

        base.Start(); // init current as base       
    }

    public override void Move()
    {
    }

    public override void Attack()
    {
        // Logic for attacking
    }

    public override bool IsDamaged()
    {
        // Logic for checking damage
        return false; // Placeholder return
    }

    public override bool IsDead()
    {
        // Logic for checking if dead
        return false; // Placeholder return
    }
}

