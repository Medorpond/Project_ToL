using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> UnitList;

    public bool isMyTurn { get; set; }
    public bool waitingCmd = false;

    private Coroutine inAction = null;

    private Character currentUnit;
    private GameObject clicked;

    private void Start()
    {
        MatchManager.Instance.onClickDown.AddListener(OnClickHold);
        MatchManager.Instance.onClickRelease.AddListener(OnClickRelease);
    }

    private void Update()
    {
        if(clicked != null) Debug.Log(clicked);
    }

    public void RegisterUnit(GameObject _unit)
    {
        UnitList.Add(_unit);
        Debug.Log($"{_unit.name} Registered under {name}");
    }

    public void RemoveUnit(GameObject _unit)
    {
        UnitList.Remove(_unit);
        Debug.Log($"{_unit.name} got Removed from {name}");
    }

    void OnClickRelease(GameObject _clicked)
    {
        if (waitingCmd) { clicked = _clicked; Debug.Log($"clicked: {clicked}"); }
        else
        {
            if (_clicked.CompareTag("MyUnit")) { currentUnit = _clicked.GetComponent<Character>(); Debug.Log($"CurUnit: {_clicked}"); }
        }
    }

    void OnClickHold(GameObject _clicked)
    {
        if (_clicked.tag == "Ally")
        {
            StartCoroutine(HoldObject());
        }
        

        IEnumerator HoldObject()
        {
            while (Input.GetMouseButton(0))
            {
                _clicked.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                yield return new WaitForSeconds(0.001f);
            }
        }
    }

    



    #region IEnumerable Acts
    IEnumerator ReadyMove()
    {
        // Highligts MoveButton
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        // Dehighlight MoveButton
        if (clicked.CompareTag("Tile"))
        {
            currentUnit.MoveTo(clicked.transform.position);
        }
        else
        {
            Debug.Log("Click On Tile to move");
        }

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
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        
        if (clicked.CompareTag("Hostile"))
        {
            currentUnit.Attack(clicked.GetComponent<Character>());
        }
        else
        {
            Debug.Log("Click On Hostile to Attack");
        }
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

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        Debug.Log("Ability1!");
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
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        Debug.Log("Ability2!");
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
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
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
}

