using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    #region enum
    public enum WeaponType { LightSword, Shield, DoubleBlade, ArrowAtk, healingMagic, HeavyAttack, MagicAttack }
    #endregion

    #region Parameter

    #region Common Parameter
    protected CommonManager player;
    protected CommonManager opponent;
    
    

    public float moveSpeed = 0.01f;
    public (int weight, string command) mostValuedAction = (0, "");

    protected WeaponType weaponType;
    public string type;
    #endregion

    #region Stat
    public float maxHealth;
    public float currentHealth;
    public float attackDamage;
    public float attackRange;
    public float moveRange;
    
    public bool isPlayer; // Indicates whether the unit belongs to the player
    #endregion

    #region Action
    protected int maxAttackCount = 1;
    protected int maxMoveCount = 1;
    protected int attackLeft;
    protected int moveLeft;
    protected int skill_1_Cooldown;
    protected int skill_2_Cooldown;
    protected int skill_1_currentCool = 0;
    protected int skill_2_currentCool = 0;
    private float defenseRate = 1.0f;

    protected bool inAction = false;

    public bool IsinAction()
    {
        return inAction;
    }
    #endregion

    #region List
    public List<Buff> buffList = new();
    public List<Node> movableNode = new();
    #endregion

    #region Graphics and Sound
    [Header("Components")]
    [SerializeField]
    protected RectTransform hp_bar;
    [SerializeField]
    protected Animator animator;
    #endregion

    protected virtual void Init()
    {
        currentHealth = maxHealth;
        attackLeft = maxAttackCount;
        moveLeft = maxMoveCount;
    }

    #endregion

    #region Unity Monobehaviour LifeCycle Method
    protected virtual void Awake() 
    {
        player = GetComponentInParent<CommonManager>() == PlayerManager.Instance 
            ? PlayerManager.Instance : OpponentManager.Instance;
        opponent = PlayerManager.Instance == player 
            ? OpponentManager.Instance : PlayerManager.Instance;
        animator = GetComponent<Animator>(); 
        Init(); 
    }
    protected virtual void Start()
    {
        ScanMovableNode();
    }

    protected void OnDestroy()
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].Remove();
        }
    }
    #endregion

    #region Methods

    #region Actions
    public bool MoveTo(Vector3 direction)
    {
        if (moveLeft <= 0 || inAction == true)
        {
            Debug.Log($"inAction: {inAction}, moveLeft: {moveLeft}");
            return false;
        }// Make sure one can't move while moving
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

        //Actual Move Starts
        moveLeft--;
        inAction = true;
        MapManager.Instance.stage.Occupy(startPos, targetPos, this);
        StartCoroutine(MoveOneGrid());
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
            ResetMoveAnimation();
            inAction = false;
            ScanMovableNode();
        }
    }

    public virtual bool Attack(Unit _opponent)
    {
        if (attackLeft <= 0) return false;

        if (Vector2.Distance(transform.position, _opponent.transform.position) > attackRange)
        {
            Debug.Log("Out of Range");
            return false;
        }
        attackLeft--;
        inAction = true;
        TriggerAttackAnimation();
        BattleAudioManager.instance.PlayWeaponSfx(weaponType);
        _opponent.IsDamaged(attackDamage);
        inAction = false;

        return true;
    }


    public virtual bool Ability1() { return false; }
    public virtual bool Ability1(Unit unit) { return false; }
    public virtual bool Ability1(GameObject target) { return false; }
    public virtual bool Ability2() { return false; }
    public virtual bool Ability2(Unit unit) { return false; }
    public virtual bool Ability2(GameObject target) { return false; }

    public bool UseDefense()
    {
        Buff defenseBuff = new Buff(2, ActiveDefense, null, DeactiveDefense, this);
        defenseBuff.Apply();
        return true;
    }

    public void ActiveDefense(Unit unit)
    {
        this.defenseRate = 0.5f;
    }

    public void DeactiveDefense(Unit unit)
    {
        this.defenseRate = 1.0f;
    }


    #endregion

    #region ReAction
    public virtual void IsDamaged(float damage)
    {
        damage = damage * defenseRate;
        currentHealth = (currentHealth - damage > 0) ? currentHealth - damage : 0;
        BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.damage);
        HP_BarUpdate();
        if (currentHealth <= 0) IsDead();
    }
    public virtual void IsHealed(float _heal)
    {
        currentHealth = currentHealth + _heal < maxHealth ? currentHealth + _heal : maxHealth;
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
        player.RemoveUnit(this);
        
        //Play Unit Death Animation HERE.
        MapManager.Instance.stage.NodeArray[(int)transform.position.x, (int)transform.position.y].isBlocked = false;
        BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.deadSound);
        // Wait Until DeathAnimation Ends
        Destroy(gameObject, 0.01f);
    }

    #endregion

    #region Highlight
    public void ShowMovableTiles()
    {
        foreach (Node node in movableNode)
        {
            GameObject tileObject = GameObject.Find($"Tile({node.x}, {node.y})");
            if (tileObject != null)
            {
                Tile tile = tileObject.GetComponent<Tile>();
                tile.ActivateHighlight();
            }
        }
    }

    public void HideMovableTiles()
    {
        foreach (Node node in movableNode)
        {
            GameObject tileObject = GameObject.Find($"Tile({node.x}, {node.y})");
            if (tileObject != null)
            {
                Tile tile = tileObject.GetComponent<Tile>();
                tile.DeactivateHighlight();
            }
        }
    }
    #endregion

    #region Others
    public void OnTurnStart()
    {
        moveLeft = maxMoveCount;
        attackLeft = maxAttackCount;

        if (skill_1_currentCool > 0) skill_1_currentCool--;
        if (skill_2_currentCool > 0) skill_2_currentCool--;

        mostValuedAction = (0, "");

        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].TurnEnd();
        }

        AnalizeAction();
    }

    protected void ScanMovableNode()
    {
        movableNode.Clear();
        Vector3Int currentPos = Vector3Int.RoundToInt(transform.position);
        int startX = (int)Mathf.Max(currentPos.x - moveRange, 0);
        int endX = (int)Mathf.Min(currentPos.x + moveRange, MapManager.Instance.stage.restrictTop.x);

        int startY = (int)Mathf.Max(currentPos.y - moveRange, 0);
        int endY = (int)Mathf.Min(currentPos.y + moveRange, MapManager.Instance.stage.restrictTop.y);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (Mathf.Abs(x - currentPos.x) + Mathf.Abs(y - currentPos.y) > moveRange) { continue; }
                int pathLength = MapManager.Instance.stage.Pathfinding(new Vector2Int(currentPos.x, currentPos.y), new Vector2Int(x, y)).Count;
                if (pathLength > 0 && pathLength <= moveRange)
                {
                    movableNode.Add(MapManager.Instance.stage.NodeArray[x, y]);
                }
            }
        }
    }

    #endregion

    #endregion

    #region Animations

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

    protected void TriggerAttackAnimation()
    {
        if (animator != null) animator.SetTrigger("Attack");
    }

    #endregion


    protected virtual void AnalizeAction() { }
}