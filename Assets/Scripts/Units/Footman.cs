using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootMan : Unit
{
    protected override void Start()
    {
        base.Start();

        // Initialize base parameters specific to FootMan
        baseHP = 100; // Example value
        baseMovePoint = 5; // Example value
        baseMoveSpeed = 2.5f; // Example value
        baseDamage = 15; // Example value
        baseRange = 1; // Example value
    }

    public override void Move()
    {
        // Logic for moving the unit
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

