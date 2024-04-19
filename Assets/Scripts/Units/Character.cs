using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

using NodeStruct;

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
    protected int moveSpeed;

    protected PathFinder pathfinder;

    // For external, Get property
    public int Health => health;
    public int AttackDamage => attackDamage;

    protected void Start()
    {
        pathfinder = PathFinder.GetInstance(); // ��ã�� �̱��� �ν��Ͻ� �ʱ�ȭ
    }


    public void MoveTo(Vector3 direction)
    {
        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)direction.x, (int)direction.y);
        // �� Vec2Int�� �ӽ÷� �Ҵ��� ����ü...

        List<Node> path = pathfinder.PathFinding(startPos, targetPos);
        // �� ��ġ���� ���������� ��� ȹ��

        foreach (Node elem in path)
        {
            Vector2 nextStop = new Vector2(elem.x, elem.y);
            MoveOneGrid(nextStop);
        } // ��ο� ����� �� ��带 ��ȯ�ϸ� MoveOneGrid() ȣ��... moveRange�� ���� �Ѱ� ���� �ʿ�.
        

        IEnumerator MoveOneGrid(Vector2 nextStop)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextStop, moveSpeed);
            yield return null;
        }// �����Լ�, �׸��� 1ĭ �̵�... ������ Vector3���� ���� ���� ���
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
            Debug.Log("���� ���� ���Դϴ�. ���� �Ұ�.");
            return false;
        }

        else return true;
    }
}
