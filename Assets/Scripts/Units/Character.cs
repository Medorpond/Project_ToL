using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

public abstract class Character : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    // character info
    public Vector2 location;
    protected int maxHealth;
    protected int health;
    protected int attackDamage;
    protected int attackRange;
    protected int moveRange;

    // For external, Get property
    public int Health => health;
    public int AttackDamage => attackDamage;




    public void MoveTo(int direction)
    {
        if (direction == 8) location += Vector2.up * moveRange;
        if (direction == 2) location -= Vector2.up * moveRange;
        if (direction == 4) location += Vector2.left * moveRange;
        if (direction == 6) location -= Vector2.left * moveRange;
    }

    public abstract void Ability();

    public virtual void Init()
    {
        transform.position = location;
        health = maxHealth;
    }

    public virtual bool DecreaseHP(int damage)
    {
        int previousHP = health;

        // make health over 0
        health = health - damage > 0 ? health - damage : 0;

        onHPEvent.Invoke(previousHP, health);

        if (health > 0) return true;
        else return false;
    }

    public virtual void IncreaseHP(int heal)
    {
        int previousHP = health;

        health = health + heal > maxHealth ? maxHealth : health + heal;

        onHPEvent.Invoke(previousHP, health);
    }

    // distance, attackRange 비교
    public bool CanAttack(float distance)
    {
        if (distance > attackRange)
        {
            Debug.Log("공격 범위 밖입니다. 공격 불가.");
            return false;
        }

        else return true;
    }
}
