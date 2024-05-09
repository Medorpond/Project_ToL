using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> UnitList;

    private bool isMyTurn;
    private bool isOperating;
    private Character atService;
    private Command currentCommand;
    private Vector3 targetPosition;
    private GameObject clicked;

    

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
    
    void ReceiveClicked(GameObject _clicked) => clicked = _clicked;

    #region IEnumerable Acts
    IEnumerator Move()
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

    IEnumerator Attack()
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

    IEnumerator Ability1()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
    }
    IEnumerator Ability2()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
    }
    IEnumerator Ability3()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
    }
    #endregion
    private enum Command
    {
        Move,
        Attack,
        Ability1,
        Ability2,
        Ability3
    }
}

