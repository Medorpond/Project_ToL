using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> UnitList = new List<GameObject>();
    public List<string> CmdList = new List<string>();

    public bool isMyTurn { get; set; }
    public bool isPlayer; // Determine if this is the player character

    private Coroutine inAction = null;

    private GameObject currentUnit;
    private GameObject clicked = null;

    private void Start()
    {
        MatchManager.Instance.onClickDown.AddListener(OnClickHold);
        MatchManager.Instance.onClickRelease.AddListener(OnClickRelease);
        
        foreach(GameObject unit in UnitList)
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
    }

    public void RegisterUnit(GameObject _unit)
    {
        UnitList.Add(_unit);
        Debug.Log($"{_unit.name} Registered under {name}");
        SetupFacingDirection(_unit); // 
    }

    public void RemoveUnit(GameObject _unit)
    {
        UnitList.Remove(_unit);
        Debug.Log($"{_unit.name} got Removed from {name}");
    }

    void OnClickRelease(GameObject _clicked)
    {
        if (inAction != null) { clicked = _clicked;}
        else if (UnitList.Contains(_clicked) && isMyTurn)
        {
            if(currentUnit != null) { currentUnit.transform.Find("ArrowPointDown").gameObject.SetActive(false); }
            currentUnit = _clicked; Debug.Log($"CurUnit: {_clicked}");
            currentUnit.transform.Find("ArrowPointDown").gameObject.SetActive(true);
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
                //Debug.Log($"{_clicked.transform.position}");
                yield return new WaitForSeconds(0.001f);
            }
        }
    }

    public void EndingTurn()
    {
        if (UnitList != null)
        {
            foreach (GameObject unit in UnitList)
            {
                unit.GetComponent<Unit>().EndTurn();
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

                if (currentUnit.GetComponent<Unit>().CheckAbilityMove(clickedPos)) currentUnit.GetComponent<Unit>().Ability1();
                else if (currentUnit.GetComponent<Unit>().MoveTo(clickedPos))
                {
                    CmdList.Add($"Move, {(int)curUnitPos.x}, {(int)curUnitPos.y}, {(int)clickedPos.x}, {(int)clickedPos.y}");
                }
            }
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
            if(inAction == null) { inAction = StartCoroutine(ReadyMove()); }
        }
    }

    IEnumerator ReadyAttack()
    {
        yield return new WaitUntil(() => clicked != null);

        if (!UnitList.Contains(clicked) && clicked.CompareTag("Unit"))// clicked.CompareTag("Opponent")
        {
            if(currentUnit != null)
            {                
                if (currentUnit.GetComponent<Unit>().Attack(clicked))
                {
                    CmdList.Add($"Attack, {(int)currentUnit.transform.position.x}, {(int)currentUnit.transform.position.y}, {(int)clicked.transform.position.x}, {(int)clicked.transform.position.y}");
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

        if (clicked.CompareTag("Unit"))// clicked.CompareTag("Opponent")
        {
            if (currentUnit != null)
            {
                if (currentUnit.GetComponent<Unit>().Ability1(clicked))
                {
                    CmdList.Add($"Ability1, {(int)currentUnit.transform.position.x}, {(int)currentUnit.transform.position.y}, {(int)clicked.transform.position.x}, {(int)clicked.transform.position.y}");
                }

                currentUnit.GetComponent<Unit>().Ability1();
            }
        }
        else
        {
            Debug.Log("Click On Hostile to Ability1");
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
                currentUnit.GetComponent<Unit>().Ability2();
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

    IEnumerator ReadyAbility3()
    {
        yield return new WaitUntil(() => clicked != null);
        Debug.Log("Ability3!");
        inAction = null;
    }
    public void Ability3()
    {
        if (isMyTurn)
        {
            if (inAction == null) { inAction = StartCoroutine(ReadyAbility3()); }
        }
    }

    #endregion

    private void UpdateFacingDirection(GameObject unit, bool isFacingRight)
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

    private void SetupFacingDirection(GameObject unit)
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
}

