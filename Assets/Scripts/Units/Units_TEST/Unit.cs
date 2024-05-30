using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public RectTransform hp_bar;
    protected Animator animator;
    public enum WeaponType { LightSword, Shield, DoubleBlade, ArrowAtk }
    protected WeaponType weaponType;

    [SerializeField]
    protected float moveSpeed = 0.01f;

    protected float maxHealth;
    protected float currentHealth;
    protected float attackDamage;
    protected float attackRange;
    protected float moveRange;

    protected int coolTime1;
    protected int coolTime2;
    protected int currentCool1;
    protected int currentCool2;
    protected bool skillActive1;
    protected bool skillActive2;
    // protected bool isSword;
    // protected bool isDoubleBlade;
    // private AudioClip attackSoundClip;

    protected Coroutine moveCoroutine;
    protected bool canAttack = true;

    protected virtual void Awake()
    {
        currentCool1 = 0;
        currentCool2 = 0;
        skillActive1 = false;
        skillActive2 = false;
        animator = GetComponent<Animator>();    
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

        MapManager.Instance.stage.Occupy(startPos, targetPos, gameObject);
        moveCoroutine = StartCoroutine(MoveOneGrid());
        TriggerMoveAnimation();
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
            ResetMoveAnimation();
        }
    }

    public virtual bool Attack(GameObject _opponent)
    {
        if(!canAttack) return false;

        //공통 기능 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (Vector2.Distance(transform.position, _opponent.transform.position) > attackRange)
        {
            Debug.Log("Out of Range");
            return false;
        }
        Debug.Log($"{name} attacked {_opponent.name}");
        TriggerAttackAnimation();
        _opponent.GetComponent<Unit>().IsDamaged(attackDamage);
        canAttack = false;
        //공통 기능 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        BattleAudioManager.instance.PlayWeaponSfx(weaponType);
        Debug.Log("Attack!");

        return true;
    }

    protected void TriggerMoveAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Move", true);
        }
    }

    protected void ResetMoveAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Move", false);
        }
    }

    protected void TriggerAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public virtual void IsDamaged(float _damage)
    {
        currentHealth = currentHealth - _damage > 0 ? currentHealth - _damage : 0;
        HP_BarUpdate();
        BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.damage);
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

    public void StartTurn()
    {
        canAttack = true;

        if (skillActive1) AfterAbility1();
        if (skillActive2) AfterAbility2();

        if (currentCool1 > 0) currentCool1--;
        if (currentCool2 > 0) currentCool2--;
    }

    public virtual void Ability1()
    {
        if (currentCool1 != 0) return;
        else
        {
            currentCool1 = coolTime1;
            skillActive1 = true;
        }
    }
    public virtual void Ability2()
    {
        if (currentCool2 != 0) return;
        else
        {
            currentCool2 = coolTime2;
            skillActive2 = true;
        }
    }

    protected abstract void AfterAbility1();
    protected abstract void AfterAbility2();

    public void ChangeAttackDamage(float damage)
    {
        attackDamage += damage;
    }
}