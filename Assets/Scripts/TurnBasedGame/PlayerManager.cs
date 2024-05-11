using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> UnitList;

    public bool isMyTurn { get; set; }
    private Character atService;
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

    void OnClickRelease(GameObject _clicked) => clicked = _clicked;

    void OnClickHold(GameObject _clicked)
    {
        if (_clicked.tag == "Unit")
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
        if (clicked.CompareTag("Node"))
        {
            atService.MoveTo(clicked.transform.position);
        }
        else
        {
            Debug.Log("Click On Tile to Move");
        }
    }
    public void Move()
    {
        if (isMyTurn)
        {
            StartCoroutine(ReadyMove());
        }
    }

    IEnumerator ReadyAttack()
    {
        // Highligts MoveButton
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        // Dehighlight MoveButton
        if (clicked.CompareTag("Opponent"))
        {
            atService.Attack(clicked.GetComponent<Character>());
        }
        else
        {
            Debug.Log("Click On Opponent to Attack");
        }
    }

    public void Attack()
    {
        if (isMyTurn)
        {
            StartCoroutine(ReadyAttack());
        }
    }


    

    IEnumerator ReadyAbility1()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        Debug.Log("Ability1!");
    }
    public void Ability1()
    {
        if (isMyTurn)
        {
            StartCoroutine(ReadyAbility1());
        }
    }

    IEnumerator ReadyAbility2()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        Debug.Log("Ability2!");
    }
    public void Ability2()
    {
        if (isMyTurn)
        {
            StartCoroutine(ReadyAbility2());
        }
    }

    IEnumerator ReadyAbility3()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        Debug.Log("Ability3!");
    }
    public void Ability3()
    {
        if (isMyTurn)
        {
            StartCoroutine(ReadyAbility3());
        }
    }

    #endregion
}

