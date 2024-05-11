using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Unit
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 120;
        currentHealth = maxHealth;
        attackDamage = 15;
        attackRange = 1;
        moveRange = 3;
    }
}
