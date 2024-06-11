using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonManager : MonoBehaviour
{
    public List<Unit> UnitList = new();

    public void RegisterUnit(Unit _unit)
    {
        UnitList.Add(_unit);
        SetupFacingDirection(_unit);
    }

    public void RemoveUnit(Unit _unit) => UnitList.Remove(_unit);

    protected void SetupFacingDirection(Unit unit)
    {
        if (unit != null)
        {
            if (this.CompareTag("Player"))
            {
                unit.isPlayer = true;
                unit.transform.localScale = new Vector3(Mathf.Abs(unit.transform.localScale.x), unit.transform.localScale.y, unit.transform.localScale.z);
            }
            else
            {
                unit.isPlayer = false;
                unit.transform.localScale = new Vector3(-Mathf.Abs(unit.transform.localScale.x), unit.transform.localScale.y, unit.transform.localScale.z);
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
}
