using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 0.01f;

    protected int maxHealth;
    protected int currentHealth;
    protected int attackDamage;
    protected int attackRange;
    protected int moveRange;

    private Coroutine moveCoroutine;
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        // Add this to PlayerManager
    }

    

    protected abstract void Init();
    public void MoveTo(Vector3 direction)
    {
        if (moveCoroutine != null) return; // Make sure one can't move while moving

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)direction.x, (int)direction.y);

        List<Node> path = MapManager.Instance.stage.Pathfinding(startPos, targetPos);

        if (path.Count > moveRange)
        {
            Debug.Log("Out of Range");
            return;
        }
        if (path.Count == 0) Debug.Log("You can't reach there.");

        MapManager.Instance.stage.Occupy(startPos, targetPos); // 유닛 관통 방지
        moveCoroutine = StartCoroutine(MoveOneGrid());
        moveCoroutine = null;

        // Local Method
        IEnumerator MoveOneGrid()
        {
            foreach (Node elem in path)
            {
                Vector3 nextStop = new Vector3(elem.x, elem.y);
                while (Vector3.Distance(transform.position, nextStop) > 0.001f)
                { transform.position = Vector3.MoveTowards(transform.position, nextStop, moveSpeed); yield return null; }
                transform.position = nextStop;
            }
        }
    }

    public virtual void Attack(Unit _opponent)
    {
        Debug.Log($"{name} attacked {_opponent.name}");
        _opponent.IsDamaged(attackDamage);
        // Trigger Animation
    }



    public virtual void IsDamaged(int _damage)
    {
        currentHealth = currentHealth - _damage > 0 ? currentHealth - _damage : 0;
        if (currentHealth <= 0) IsDead();
        // Invoke Event to Trigger Animation, Update UI.
    }

    public virtual void IsHealed(int _heal)
    {
        currentHealth = currentHealth + _heal < maxHealth ? 
            currentHealth + _heal : maxHealth;
        // Invoke Event to Trigger Animation, Update UI.
    }


    public virtual void IsDead()
    {
        // Remove this from PlayerManager's List<Character>;
        // Wait Until DeathAnimation Ends


        Destroy(gameObject, 0);
    }

}
