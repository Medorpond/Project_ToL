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
    public Vector3 location;
    protected int maxHealth;
    protected int health;
    protected int attackDamage;
    protected int moveRange;

    // For external, Get property
    public int Health => health;
    public int AttackDamage => attackDamage;



    // temp direction ( up : 1, down : 2, right : 3, left : 4)
    public abstract void MoveTo(int direction);
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
}
