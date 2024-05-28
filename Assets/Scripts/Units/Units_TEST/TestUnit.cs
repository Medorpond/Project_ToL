using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : Unit
{
    private float smashRange = 5;

    protected override void Awake()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        attackDamage = 5;
        attackRange = 3;
    }







    protected override void Init() { }
    protected override void AfterAbility1() { }
    protected override void AfterAbility2() { }
}
