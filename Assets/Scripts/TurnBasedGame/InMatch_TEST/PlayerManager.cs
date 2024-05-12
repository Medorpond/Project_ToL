using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> UnitList;

    public bool isMyTurn { get; set; }

    private Coroutine inAction = null;

    private GameObject currentUnit;
    private GameObject clicked = null;

    private void Start()
    {
        MatchManager.Instance.onClickDown.AddListener(OnClickHold);
        MatchManager.Instance.onClickRelease.AddListener(OnClickRelease);
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
    }

    public void RemoveUnit(GameObject _unit)
    {
        UnitList.Remove(_unit);
        Debug.Log($"{_unit.name} got Removed from {name}");
    }

    void OnClickRelease(GameObject _clicked)
    {
        if (inAction != null) { clicked = _clicked; Debug.Log($"clicked: {clicked}"); }
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

    #region IEnumerable Acts
    IEnumerator ReadyMove()
    {
        yield return new WaitUntil(() => clicked != null);

        if (clicked.CompareTag("Tile"))
        {
            if(currentUnit != null)
            {
                currentUnit.GetComponent<Unit>().MoveTo(clicked.transform.position);
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
                currentUnit.GetComponent<Unit>().Attack(clicked.GetComponent<Unit>());
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
        yield return new WaitUntil(() => clicked != null);
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
}

