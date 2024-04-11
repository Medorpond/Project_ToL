using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int> { }

public abstract class Character : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    // character info
    public Vector3 position;
    protected int maxHealth;
    protected int health;
    protected int attackDamage;

    // For external, Get property
    public int Health => health;
    public int AttackDamage => attackDamage;



    public abstract void MoveTo();
    public abstract void Ability();

    public virtual void Init()
    {
        transform.position = position;
        health = maxHealth;
    }

    public virtual bool DecreaseHP(int damage)
    {
        // make health over 0
        health = health - damage > 0 ? health - damage : 0;

        onHPEvent.Invoke(health);

        if (health > 0) return true;
        else return false;
    }

    public virtual void IncreaseHP(int heal)
    {
        health = health + heal > maxHealth ? maxHealth : health + heal;

        onHPEvent.Invoke(health);
    }
}
