using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Unit
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 7;
        currentHealth = maxHealth;
        attackDamage = 2;
        attackRange = 3;
        moveRange = 4;
    }
}
