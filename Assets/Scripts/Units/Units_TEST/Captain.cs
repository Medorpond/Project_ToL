using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Unit
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Init()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        attackDamage = 4;
        attackRange = 1;
        moveRange = 30;
    }

    public override void IsDead()
    {
        // Trigger Death Animation
        MatchManager.Instance.GameOver();
    }
}
