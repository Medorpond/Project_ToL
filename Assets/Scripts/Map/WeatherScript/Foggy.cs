using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foggy : Weather
{
    protected override void OnApply(Unit unit)
    {
        unit.moveRange = unit.moveRange - 1 > 0 ? unit.moveRange - 1 : 0;
        unit.attackDamage += 1;

        Debug.Log($"Foggy Weather applied to {unit.name}!");
    }

    protected override void OnTurnEnd(Unit unit)
    {
        Debug.Log($"Foggy Weather effected {unit.name} on turn End!");
    }

    protected override void OnRemove(Unit unit)
    {
        unit.moveRange = unit.originalMoveRange;
        unit.attackDamage -= 1;

        Debug.Log($"Foggy Weather removed from {unit.name}!");
    }
}
