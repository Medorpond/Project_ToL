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
        maxHealth = 3;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 5; 
        moveRange = 7;
    }
}
