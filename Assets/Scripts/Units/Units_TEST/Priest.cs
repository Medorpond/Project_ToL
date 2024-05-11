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
        maxHealth = 70;
        currentHealth = maxHealth;
        attackDamage = 10;
        attackRange = 3;
        moveRange = 4;
    }
}
