using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    // Properties ::
    [Header("Properties")]
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private float moveSpeed;
    private float movePoint;
    private float range;
    private float damage;
    // :: Properties
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Methods ::
    // Battle ::
    public void Attack(Unit _opponent)
    {
        _opponent.isDamaged(damage);
    }
    public void isDamaged(float _damage)
    {
        if(_damage >= currentHP)
        {
            currentHP = 0f;
            isDead();
        }
        else { currentHP -= _damage; }
    } //Damaging process

    public void isDead()
    {
        Debug.Log(this.name + " is Dead!");
    }
    // :: Battle

    // Move ::
    public void Move(float _destX, float _destY)
    {
        // get Current Location, get destination's Location, subtract, move.
        Debug.Log(this.name + " Moved to " + _destX + ", " + _destY);
    }
    // :: Move

    // :: Methods
}
