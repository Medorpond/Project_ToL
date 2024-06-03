using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeStruct;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public RectTransform hp_bar;
    protected Animator animator;
    public enum WeaponType { LightSword, Shield, DoubleBlade, ArrowAtk, healingMagic, HeavyAttack }
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
    protected bool canMove = true;

    public bool isPlayer; // Indicates whether the until belongs to the player

    public List<Buff> buffList = new List<Buff>();

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
        MatchManager.Instance.onTurnEnd.AddListener(OnTurnEnd);
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
        canMove = false;
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
        StartCoroutine(ResetAttackCooldown());
        //공통 기능 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        BattleAudioManager.instance.PlayWeaponSfx(weaponType);
        Debug.Log("Attack!");

        return true;
    }

    private IEnumerator ResetAttackCooldown()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        canAttack = true;
    }

    protected void TriggerAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    protected void TriggerMoveAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Move", true);
            BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.movingOnGrass);
        }
    }

    protected void ResetMoveAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Move", false);
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
        BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.deadSound);
        
        Destroy(gameObject, 0.01f);

    }

    public void StartTurn()
    {
        canAttack = true;
        canMove = true;

        if (skillActive1) AfterAbility1();
        if (skillActive2) AfterAbility2();

        if (currentCool1 > 0) currentCool1--;
        if (currentCool2 > 0) currentCool2--;
    }

    public virtual void Ability1()
    {

    }

    public virtual bool Ability1(GameObject opponent) //오버로딩
    {
        return false; // 임시
    }

    public virtual void Ability2()
    {
        
    }

    protected abstract void AfterAbility1();
    protected abstract void AfterAbility2();

    public void ChangeAttackDamage(float damage)
    {
        attackDamage += damage;
    }

    public bool CheckAbilityMove(Vector3 direction)
    {
        Archer archer = GetComponent<Archer>();
        Captain captain = GetComponent<Captain>();

        if (skillActive1)
        {
            if (archer != null)
            {
                archer.skillDirection = direction;
                return true;
            }
            if (captain != null)
            {
                captain.skillDirection = direction;
                return true;
            }
        }
        return false;
    }

    protected void OnTurnEnd()
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].TurnEnd();
        }//올바른 참조를 위한 역순참조
        // Make Boolean for each Action true;
    }

    protected void OnDestroy()
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].Remove();
        }
        if (MatchManager.Instance != null)
        {
            MatchManager.Instance.onTurnEnd.RemoveListener(OnTurnEnd);
        }
    }
}