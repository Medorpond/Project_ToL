using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : Unit
{

    [SerializeField]
    private RangeScan attackScan;
    [SerializeField]
    private RangeScan abilityScan_1;

    private PlayerManager parent;

    protected override void Awake()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        attackDamage = 5;
        attackRange = 3;
        moveRange = 100;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject obj in attackScan.inRange)
            {
                Debug.Log(obj.name);
            }
        }
    }
    public override bool Attack(GameObject enemy)
    {
        if (attackScan.inRange.Contains(enemy))
        {
            Debug.Log($"{enemy.name} got Attacked!");
            return true;
        }
        else
        {
            Debug.Log($"{enemy.name} not in Range!");
            return false;
        }
    }

    public bool Smash(GameObject enemy)
    {
        if (abilityScan_1.inRange.Contains(enemy))
        {
            Debug.Log($"{enemy.name} got Smashed!");
            return true;
        }
        else
        {
            Debug.Log($"{enemy.name} not in Range!");
            return false;
        }
    }

    protected override void Init() { }
    protected override void AfterAbility1() { }
    protected override void AfterAbility2() { }
}
