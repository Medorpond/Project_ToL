using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public RectTransform hp_bar;

    [SerializeField]
    protected float moveSpeed = 0.01f;

    protected float maxHealth;
    protected float currentHealth;
    protected float attackDamage;
    protected float attackRange;
    protected float moveRange;

    private Coroutine moveCoroutine;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        // Add this to PlayerManager
    }

    protected abstract void Init();

    public bool MoveTo(Vector3 direction)
    {
        if (moveCoroutine != null) return false; // Make sure one can't move while moving

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int targetPos = new Vector2Int((int)direction.x, (int)direction.y);

        List<Node> path = MapManager.Instance.stage.Pathfinding(startPos, targetPos);

        if (path.Count > moveRange)
        {
            Debug.Log("Too Far to Go");
            return false;
        }
        if (path.Count == 0)
        {
            Debug.Log("You can't reach there.");
            return false;
        }

        MapManager.Instance.stage.Occupy(startPos, targetPos, gameObject); // 유닛 관통 방지
        moveCoroutine = StartCoroutine(MoveOneGrid());
        return true;

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
            moveCoroutine = null;
        }
    }

    public virtual bool Attack(GameObject _opponent)
    {
        if (Vector2.Distance(transform.position, _opponent.transform.position) > attackRange)
        {
            Debug.Log("Out of Range");
            return false;
        }
        Debug.Log($"{name} attacked {_opponent.name}");
        _opponent.GetComponent<Unit>().IsDamaged(attackDamage);
        return true;
        // Trigger Animation
    }



    public virtual void IsDamaged(float _damage)
    {
        currentHealth = currentHealth - _damage > 0 ? currentHealth - _damage : 0;
        HP_BarUpdate();
        if (currentHealth <= 0) IsDead();
        // Invoke Event to Trigger Animation, Update UI.
    }

    public virtual void IsHealed(float _heal)
    {
        currentHealth = currentHealth + _heal < maxHealth ? 
            currentHealth + _heal : maxHealth;
        HP_BarUpdate();        
        // Invoke Event to Trigger Animation, Update UI.
    }

    private void HP_BarUpdate()
    {
        float widthratio = currentHealth / maxHealth;
        Debug.Log(widthratio);
        hp_bar.sizeDelta = new Vector2(0.5f * widthratio, hp_bar.sizeDelta.y);
    }

    public virtual void IsDead()
    {
        // Remove this from PlayerManager's List<Character>;
        // Wait Until DeathAnimation Ends

        MapManager.Instance.stage.NodeArray[(int)transform.position.x, (int)transform.position.y].isBlocked = false;
        Destroy(gameObject, 0);
    }
}
