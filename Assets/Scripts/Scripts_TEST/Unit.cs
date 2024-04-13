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
    protected int moveSpeed;

    protected bool canMove;
    protected bool canAttack;

    private Vector2Int location;

    private PathFinder pathfinder;
    #endregion




    void Start()
    {
        pathfinder = PathFinder.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Move(Vector2Int _destination)
    {
        int moveLeft = movePoint;
        List<Node> Path = pathfinder.PathFinding(location, _destination);
        if (Path.Count > movePoint) { Debug.Log("Out of Range!"); return; }
        else
        {
            while (moveLeft > 0)
            {
                int pathIndex = movePoint - moveLeft;
                Vector2Int nextNode = new Vector2Int(Path[pathIndex].x, Path[pathIndex].y);
                MoveOneGrid(location, nextNode);
                location = new Vector2Int(nextNode.x, nextNode.y);
                moveLeft -- ;
            }
        }
        
    }

    private IEnumerable MoveOneGrid(Vector2Int location, Vector2Int destination)
    {
        transform.position = Vector2.MoveTowards(location, destination, moveSpeed * Time.deltaTime);
        yield return new WaitUntil(() => 
        (transform.position.x == destination.x && transform.position.y == destination.y));
    }

    protected virtual void Attack(Unit _enemy)
    {
        _enemy.isDamaged(ATK);
    }

    public virtual void isDamaged(int _damage)
    {
        if (currentHP - _damage > 0) { currentHP -= _damage; }
        else { currentHP = 0; isDead(); }
    }

    protected virtual void isDead()
    {
        Debug.Log(name + " is Down!");
        Destroy(this, 3); // 사망애니메이션 3초
    }
}
