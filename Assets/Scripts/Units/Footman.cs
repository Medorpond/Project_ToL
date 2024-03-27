using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footman : Unit
{
    protected override float maxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    protected override float currentHP
    {
        get { return currentHP; }
        set 
        {
            if(value < 0) { value = 0; }
            currentHP = value;
        }
    }
    protected override float moveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    protected override float movePoint
    {
        get { return movePoint; }
        set { movePoint = value; }
    }
    protected override float range
    {
        get { return range; }
        set { range = value; }
    }// 아직 미작성
    protected override float damage
    {
        get { return damage; }
        set { damage = value; }
    }// 아직 미작성

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
