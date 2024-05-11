using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 70;
        currentHealth = maxHealth;
        attackDamage = 25;
        attackRange = 5;
        moveRange = 2;
    }
}
