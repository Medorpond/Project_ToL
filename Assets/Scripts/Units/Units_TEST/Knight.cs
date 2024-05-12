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
        maxHealth = 12;
        currentHealth = maxHealth;
        attackDamage = 3;
        attackRange = 1;
        moveRange = 3;
    }
}
