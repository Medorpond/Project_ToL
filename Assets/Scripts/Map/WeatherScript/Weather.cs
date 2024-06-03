using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weather
{
    public void ApplyWeatherEffect(List<GameObject> unitList)
    {
        foreach (GameObject unit in unitList)
        {
            Unit unitScript = unit.GetComponent<Unit>();
            if (unitScript != null)
            {
                Buff buff = new Buff(99, OnApply, OnTurnEnd, OnRemove, unitScript);
                buff.Apply();
            }
        }
    }

    protected virtual void OnApply(Unit unit) { }
    protected virtual void OnTurnEnd(Unit unit) { }
    protected virtual void OnRemove(Unit unit) { }
}
