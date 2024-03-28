using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ReactOnClick
{
    [SerializeField]
    private GameManager gameManager;
    #region parameter
    protected float baseHP;
    protected float baseMovePoint;
    protected float baseMoveSpeed;
    protected float baseDamage;
    protected float baseRange;

    [SerializeField] protected float currentHP;
    [SerializeField] protected float currentMovePoint;
    [SerializeField] protected float currentMoveSpeed;
    [SerializeField] protected float currentDamage;
    [SerializeField] protected float currentRange;
    #endregion

    protected virtual void Start()
    {
        // init currentParam to baseParam
        currentHP = baseHP;
        currentMovePoint = baseMovePoint;
        currentMoveSpeed = baseMoveSpeed;
        currentDamage = baseDamage;
        currentRange = baseRange;
    }

    #region Method
    public abstract void Move();

    public abstract void Attack();

    public abstract bool IsDamaged();

    public abstract bool IsDead();

    public void OnClick()
    {
        gameManager.UnitSelected = gameObject;
        Debug.Log(name + " is Selected!");
    }
    #endregion
}
