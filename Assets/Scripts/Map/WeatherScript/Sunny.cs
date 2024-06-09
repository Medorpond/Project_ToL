using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunny : Weather
{
    protected override void OnApply(Unit unit)
    {
        unit.attackRange += 3;

        Debug.Log($"Sunny Weather applied to {unit.name}!");
    }

    protected override void OnTurnEnd(Unit unit)
    {
        Debug.Log($"Sunny Weather effected {unit.name} on turn End!");
    }

    protected override void OnRemove(Unit unit)
    {
        unit.attackRange = unit.attackRange - 3 > 0 ? unit.attackRange : 0;

        Debug.Log($"Sunny Weather removed from {unit.name}!");
    }
}
