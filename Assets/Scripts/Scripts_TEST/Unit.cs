using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public abstract class Unit : MonoBehaviour
{
    #region Parameter
    protected int maxHP;
    protected int currentHP;
    protected int ATK;
    protected int atkRange;
    protected int movePoint;
    public int moveSpeed;

    protected bool canMove;
    protected bool canAttack;

    protected Vector2Int location;

    protected PathFinder pathfinder;
    #endregion




    protected virtual void Start()
    {
        canMove = true;
        canAttack = true;
        location = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        pathfinder = PathFinder.GetInstance();
    }

    protected virtual void Update()
    {
        // Actions...
    }

    public virtual IEnumerator Move(Vector2Int _destination)
    {
        int movePointLeft = movePoint;
        List<Node> Path = pathfinder.PathFinding(location, _destination);
        if (Path.Count > movePoint) { Debug.Log("Out of Range!"); yield return null; }
        else if (Path.Count == 0) { Debug.Log("Move not to Move"); yield return null; }
        else
        {
            while (movePointLeft > 0)
            {
                int pathIndex = movePoint - movePointLeft;
                Vector2Int nextNode = new Vector2Int(Path[pathIndex].x, Path[pathIndex].y);
                yield return StartCoroutine(MoveOneGrid(location, nextNode));
                location = new Vector2Int(nextNode.x, nextNode.y);
                movePointLeft -- ;
            }
        }
        
    }

    private IEnumerator MoveOneGrid(Vector2Int location, Vector2Int destination)
    {
        float startTime = Time.time;
        Vector2 startPosition = transform.position;
        float tripLength = Vector2.Distance(startPosition, destination);

        while ((Vector2)transform.position != destination)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float distFraction = distCovered / tripLength;
            transform.position = Vector2.Lerp(startPosition, destination, distFraction);
            yield return null;
        }
    }

    public virtual void Attack(Unit _enemy)
    {
        if (Mathf.Abs(_enemy.location.x - location.x) + Mathf.Abs(_enemy.location.y - location.y) <= 1)
        {
            _enemy.isDamaged(ATK);
        }
        else { Debug.Log("OutOfRange!"); }
        
    }

    public virtual void isDamaged(int _damage)
    {
        if (currentHP - _damage > 0) { Debug.Log(name + ": " + currentHP); currentHP -= _damage; }
        else { currentHP = 0; isDead(); }
    }

    protected virtual void isDead()
    {
        Debug.Log(name + " is Down!");
        currentHP = maxHP; // Immortal, for Test.
        // Destroy(this, 3); // 사망애니메이션 3초
    }
}
