using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        maxHP = 100;
        currentHP = maxHP;
        ATK = 30;
        atkRange = 1;
        movePoint = 300;
        moveSpeed = 5;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
