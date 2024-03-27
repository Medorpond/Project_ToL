using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    // Base parameters
    protected float baseHP;
    protected float baseMovePoint;
    protected float baseMoveSpeed;
    protected float baseDamage;
    protected float baseRange;

    // Current parameters serialized for editor visibility but protected to prevent external access
    [SerializeField] protected float currentHP;
    [SerializeField] protected float currentMovePoint;
    [SerializeField] protected float currentMoveSpeed;
    [SerializeField] protected float currentDamage;
    [SerializeField] protected float currentRange;

    protected virtual void Start()
    {
        // Initialize current values to base values
        currentHP = baseHP;
        currentMovePoint = baseMovePoint;
        currentMoveSpeed = baseMoveSpeed;
        currentDamage = baseDamage;
        currentRange = baseRange;
    }

    public abstract void Move();

    public abstract void Attack();

    public abstract bool IsDamaged();

    public abstract bool IsDead();
}
