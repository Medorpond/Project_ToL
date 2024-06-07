using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainy : Weather
{
    protected override void OnApply(Unit unit)
    {
        unit.moveRange += 2;
        unit.attackRange = unit.attackRange - 2 > 1 ? unit.attackRange - 2 : 1;

        Debug.Log($"Rainy Weather applied to {unit.name}!");
    }

    protected override void OnTurnEnd(Unit unit)
    {
        Debug.Log($"Rainy Weather effected {unit.name} on turn End!");
    }

    protected override void OnRemove(Unit unit)
    {
        unit.moveRange = unit.originalMoveRange;
        unit.attackRange = unit.originalAttackRange;

        Debug.Log($"Rainy Weather removed from {unit.name}!");
    }
}
