using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

using NodeStruct;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

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

    protected float moveSpeed = 0.01f;

    private MousePosition mousePosition;

    //Pseudo Coordinate
    public int x;
    public int y;
    private Vector3 destination;
    public bool canMove;


    // For external, Get property
    public int Health => health;
    public int AttackDamage => attackDamage;
    public int MoveRange => moveRange;



    protected void Start()
    {
        mousePosition = GameObject.Find("TurnBasedGameManage").GetComponent<MousePosition>();
        canMove = false;
    }

    protected void Update()
    {
        location = transform.position;

        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                destination = new Vector3 (Mathf.Round(mousePosition.SwitchToNode().x), Mathf.Round(mousePosition.SwitchToNode().y));
                MoveTo(destination);
                // can move only Once
                canMove = false;
            }
        }
    }

    public void MoveTo(Vector3 direction)
    {
        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)direction.x, (int)direction.y);

        List<Node> path = MapManager.Instance.stage.Pathfinding(startPos, targetPos);
        MapManager.Instance.stage.Occupy(startPos, targetPos); // 유닛 관통 방지

        StartCoroutine(MoveOneGrid());
        
        IEnumerator MoveOneGrid()
        {
            foreach (Node elem in path)
            {
                Vector3 nextStop = new Vector3(elem.x, elem.y);
                while(Vector3.Distance(transform.position, nextStop) > 0.001f) 
                { transform.position = Vector3.MoveTowards(transform.position, nextStop, moveSpeed); yield return null; }
                transform.position = nextStop;
            }
        }
    }

    public virtual void Init()
    {
        transform.position = location;
        health = maxHealth;
    }
    /*
    public virtual bool DecreaseHP(int damage)
    {
        int previousHP = health;

        // make health over 0
        health = health - damage > 0 ? health - damage : 0;

        onHPEvent.Invoke(previousHP, health);

        if (health > 0) return true;
        else return false;
    }
    */
    public void Attack(Character _opponent)
    {
        _opponent.getDamage(attackDamage);
    }

    //public abstract void Ability1();
    //public abstract void Ability2();
    //public abstract void Ability3();

    public virtual void getDamage(int atk)
    {
        int previousHP = health;
        health = health - atk > 0 ? health - atk : 0;
        onHPEvent.Invoke(previousHP, health);

        if(health <= 0)
        {
            health = 0;
            isDead();
        }
    }

    public void isDead() 
    {
        // parent.RemoveUnit(this); Remove this from parent(PlayerManager)'s List
        Destroy(this, 0);
    }
    public virtual void IncreaseHP(int heal)
    {
        int previousHP = health;

        health = health + heal > maxHealth ? maxHealth : health + heal;

        onHPEvent.Invoke(previousHP, health);
    }

    public bool InRange(float distance)
    {
        if (distance > attackRange)
        {
            Debug.Log("Can't Attack. Out of Range.");
            return false;
        }

        else return true;
    }
}
