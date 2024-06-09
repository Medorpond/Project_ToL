using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weather
{
    public void ApplyWeatherEffect(List<Unit> unitList)
    {
        foreach (Unit unit in unitList)
        {
            if (unit != null)
            {
                Buff buff = new Buff(99, OnApply, OnTurnEnd, OnRemove, unit);
                buff.Apply();
            }
        }
    }

    protected virtual void OnApply(Unit unit) { }
    protected virtual void OnTurnEnd(Unit unit) { }
    protected virtual void OnRemove(Unit unit) { }
}
