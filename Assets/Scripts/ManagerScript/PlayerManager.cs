using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public List<Unit> UnitList = new();
    public List<string> CmdList = new();

    public bool isMyTurn { get; set; }
    public bool isPlayer; // Determine if this is the player character

    private Coroutine inAction = null;

    private Unit currentUnit;
    private GameObject clicked = null;

    public Button moveButton;


    #region Singletone
    private static PlayerManager instance = null;
    public static PlayerManager Instance
    {
        get
        {

            if (instance == null)
            {
                instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    private void Awake()
    {
        SingletoneInit();
    }
    private void Start()
    {
        MatchManager.Instance.onClickDown.AddListener(OnClickHold);
        MatchManager.Instance.onClickRelease.AddListener(OnClickRelease);

        
        foreach(Unit unit in UnitList)
        {
            SetupFacingDirection(unit);
        }
    }

    private void Update()
    {
        if (!isMyTurn && currentUnit != null)
        {
            currentUnit.transform.Find("ArrowPointDown").gameObject.SetActive(false);
        }

        //yong
        // click m : move, click a : Attack
        // color change yet
        if (isMyTurn && currentUnit != null && Input.GetKeyDown(KeyCode.M))
        {
            Move();
        }

        if (isMyTurn && currentUnit != null && Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
        //yong
    }

    public void RegisterUnit(Unit _unit)
    {
        UnitList.Add(_unit);
        SetupFacingDirection(_unit);
    }

    public void RemoveUnit(Unit _unit)
    {
        UnitList.Remove(_unit);
        Debug.Log($"{_unit.name} got Removed from {name}");
    }

    void OnClickRelease(GameObject _clicked)
    {
        if (inAction != null) { clicked = _clicked;}
        else
        {
            Unit _currentUnit = _clicked.GetComponent<Unit>();
            if (UnitList.Contains(_currentUnit) && isMyTurn)
            {
                if (currentUnit != null) { currentUnit.transform.Find("ArrowPointDown").gameObject.SetActive(false); }
                currentUnit = _currentUnit; Debug.Log($"CurUnit: {_clicked}");
                currentUnit.transform.Find("ArrowPointDown").gameObject.SetActive(true);
            }
        } 
    }

    void OnClickHold(GameObject _clicked)
    {
        if (_clicked.CompareTag("Unit"))
        {
            StartCoroutine(HoldObject());
        }
        

        IEnumerator HoldObject()
        {
            while (Input.GetMouseButton(0))
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = -1;
                _clicked.transform.position = position;
                yield return new WaitForSeconds(0.001f);
            }
        }
    }

    public void EndingTurn()
    {
        if (UnitList != null)
        {
            foreach (Unit unit in UnitList)
            {
                unit.OnTurnStart();

                // should add line code
            }
        }
    }

    #region IEnumerable Acts
    

    IEnumerator ReadyMove()
    {
        yield return new WaitUntil(() => clicked != null);
        if (clicked.CompareTag("Tile"))
        {
            if (currentUnit != null)
            {
                Vector3 curUnitPos = currentUnit.transform.position;
                Vector3 clickedPos = clicked.transform.position;

                // Determine the direction to move
                Vector3 direction = clickedPos - curUnitPos;
                bool isFacingRight = direction.x >= 0;

                // Update the facing direction of the unit
                UpdateFacingDirection(currentUnit, isFacingRight);

                if (currentUnit.MoveTo(clickedPos))
                {
                    CmdList.Add($"@Move/({(int)curUnitPos.x},{(int)curUnitPos.y})/({(int)clickedPos.x},{(int)clickedPos.y})");
                }
                else { Debug.Log("Cannot Move"); }

                UnhighlightMovableTiles(currentUnit);
            }
            else Debug.Log("CurrentUnit is Null");
        }
        else
        {
            Debug.Log("Click On Tile to move");
        }

        clicked = null;
        inAction = null;
    }

   
    public void Move()
    {
        if (isMyTurn)
        {
            if (currentUnit != null) { HighlightMovableTiles(currentUnit);  }

            if (inAction == null) { inAction = StartCoroutine(ReadyMove()) ; }
        }
    }

    IEnumerator ReadyAttack()
    {
        yield return new WaitUntil(() => clicked != null);
        Unit target = clicked.GetComponent<Unit>();
        if (!UnitList.Contains(target) && clicked.CompareTag("Unit"))// OpponentManager.EnemyList.Contains(clicked)
        {
            if(currentUnit != null)
            {
                // Determine the direction to face
                Vector3 direction = clicked.transform.position - currentUnit.transform.position;
                bool isFacingRight = direction.x >= 0;

                // Update the facing direction of the unit
                UpdateFacingDirection(currentUnit, isFacingRight);

                if (currentUnit.Attack(target))
                {
                    CmdList.Add($"@Attack/({(int)currentUnit.transform.position.x},{(int)currentUnit.transform.position.y})/({(int)target.transform.position.x},{(int)target.transform.position.y})");
                }
            }   
        }
        else
        {
            Debug.Log("Click On Hostile to Attack");
        }


        clicked = null;
        inAction = null;
    }

    public void Attack()
    {
        if (isMyTurn)
        {
            if (inAction == null) { inAction = StartCoroutine(ReadyAttack()); }
        }
    }


    

    IEnumerator ReadyAbility1()
    {

        yield return new WaitUntil(() => clicked != null);

        if (clicked.CompareTag("Tile"))
        {
            if (currentUnit != null)
            {
                if (currentUnit.Ability1(clicked))
                {
                    CmdList.Add($"@Ability1/({(int)currentUnit.transform.position.x},{(int)currentUnit.transform.position.y})/({(int)clicked.transform.position.x},{(int)clicked.transform.position.y})");
                }

                currentUnit.Ability1();
            }
        }
        else
        {
            Debug.Log("Click On Hostile to Use Ability1");
        }


        clicked = null;
        inAction = null;
    }
    public void Ability1()
    {
        if (isMyTurn)
        {
            if (inAction == null) { inAction = StartCoroutine(ReadyAbility1()); }
        }
    }

    IEnumerator ReadyAbility2()
    {
        yield return new WaitUntil(() => clicked != null);

        if (clicked.CompareTag("Unit"))// clicked.CompareTag("Opponent")
        {
            if (currentUnit != null)
            {
                currentUnit.Ability2();
            }

        }

        clicked = null;
        inAction = null;
    }
    public void Ability2()
    {
        if (isMyTurn)
        {
            if (inAction == null) { inAction = StartCoroutine(ReadyAbility2()); }
        }
    }

    #endregion

    private void UpdateFacingDirection(Unit unit, bool isFacingRight)
    {
        Vector3 newScale = unit.transform.localScale;
        if (isFacingRight)
        {
            newScale.x = Mathf.Abs(newScale.x);
        }
        else
        {
            newScale.x = -Mathf.Abs(newScale.x);
        }
        unit.transform.localScale = newScale;
    }

    private void SetupFacingDirection(Unit unit)
    {
        if (unit != null)
        {
            if (isPlayer)
            {
                unit.GetComponent<Unit>().isPlayer = true;
                unit.transform.localScale = new Vector3(Mathf.Abs(unit.transform.localScale.x), unit.transform.localScale.y, unit.transform.localScale.z);
            }
            else
            {
                unit.GetComponent<Unit>().isPlayer = false;
                unit.transform.localScale = new Vector3(-Mathf.Abs(unit.transform.localScale.x), unit.transform.localScale.y, unit.transform.localScale.z);
            }
        }
    }


    private void HighlightMovableTiles(Unit unit)
    {
        Debug.Log("Highlighting movable tiles");
        Unit unitComponent = unit.GetComponent<Unit>();
        int moveRange = (int)unitComponent.moveRange; // Ensure moveRange is int

        Vector3 unitPosition = unit.transform.position;

        for (int x = -moveRange; x <= moveRange; x++)
        {
            for (int y = -moveRange; y <= moveRange; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= moveRange)
                {
                    Vector3 tilePosition = new Vector3(unitPosition.x + x, unitPosition.y + y, unitPosition.z);
                    int tilePosX = Mathf.RoundToInt(tilePosition.x);
                    int tilePosY = Mathf.RoundToInt(tilePosition.y);
                    GameObject tileObject = GameObject.Find($"Tile({tilePosX}, {tilePosY})");
                    if (tileObject != null && !IsTileOccupiedByOtherCharacter(tilePosition))
                    {
                        Tile tile = tileObject.GetComponent<Tile>();
                        tile.ActivateHighlight();
                    }
                }
            }
        }
    }

    // Helper method to check if a tile is occupied by another character
    private bool IsTileOccupiedByOtherCharacter(Vector3 tilePosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tilePosition, 0.1f); // Adjust the radius according to your character size
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Unit") && collider.gameObject != currentUnit)
            {
                return true;
            }
        }
        return false;
    }

    private void UnhighlightMovableTiles(Unit unit)
    {
        Debug.Log("Unhighlighting movable tiles");
        Unit unitComponent = unit.GetComponent<Unit>();
        int moveRange = (int)unitComponent.moveRange; // Ensure moveRange is int

        Vector3 unitPosition = unit.transform.position;

        for (int x = -moveRange; x <= moveRange; x++)
        {
            for (int y = -moveRange; y <= moveRange; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= moveRange)
                {
                    Vector3 tilePosition = new Vector3(unitPosition.x + x, unitPosition.y + y, unitPosition.z);
                    int tilePosX = Mathf.RoundToInt(tilePosition.x); // Convert float to int
                    int tilePosY = Mathf.RoundToInt(tilePosition.y); // Convert float to int
                    GameObject tileObject = GameObject.Find($"Tile({tilePosX}, {tilePosY})");
                    if (tileObject != null)
                    {
                        Tile tile = tileObject.GetComponent<Tile>();
                        tile.DeactivateHighlight();

                    }
                }
            }
        }
    }

}

