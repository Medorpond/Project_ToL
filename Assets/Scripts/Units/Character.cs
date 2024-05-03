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

    [SerializeField]
    protected float moveSpeed = 0.05f;

    private MousePosition mousePosition;

    //Pseudo Coordinate
    public int x;
    public int y;
    private Vector3 destination;

    protected PathFinder pathfinder;

    // For external, Get property
    public int Health => health;
    public int AttackDamage => attackDamage;
    public int MoveRange => moveRange;



    protected void Start()
    {
        pathfinder = PathFinder.GetInstance(); // ��ã�� �̱��� �ν��Ͻ� 
        mousePosition = GameObject.Find("TurnBasedGameManage").GetComponent<MousePosition>();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            destination = new Vector3(x, y); // Pseudo Coordinate.
            MoveTo(destination);
        }
        
        /*
        if (Input.GetMouseButtonDown(0))
        {
            if (mousePosition.CheckRange())
            {
                destination = mousePosition.SwitchToNode();
                MoveTo(destination);
                Debug.Log($"({destination.x}, {destination.y})");
            }
        }
        */
    }

    public void MoveTo(Vector3 direction)
    {
        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)direction.x, (int)direction.y);

        List<Node> path = pathfinder.PathFinding(startPos, targetPos);
        // �� ��ġ���� ���������� ��� �迭 ȹ��

        StartCoroutine(MoveOneGrid());
        
        IEnumerator MoveOneGrid()
        {
            foreach (Node elem in path)
            {
                Vector3 nextStop = new Vector3(elem.x, elem.y);
                while(Vector3.Distance(transform.position, nextStop) > 0.001f) 
                { transform.position = Vector3.MoveTowards(transform.position, nextStop, moveSpeed); yield return null; } 
            } // ��ο� ����� �� ��带 ��ȯ�ϸ� �� ĭ�� �̵�.
        }
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

    // distance, attackRange ��
    public bool CanAttack(float distance)
    {
        if (distance > attackRange)
        {
            Debug.Log("Can't Attack. Out of Range.");
            return false;
        }

        else return true;
    }
}
