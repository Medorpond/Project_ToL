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
        maxHealth = 4;
        currentHealth = maxHealth;
        attackDamage = 0;
        attackRange = 0;
        moveRange = 4;
    }
}
