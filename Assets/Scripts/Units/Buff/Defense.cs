using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : Buff
{
    public Defense(int _persistTurn, Unit _buffHolder) : base(_persistTurn, null, null, null, _buffHolder)
    {
        onApply = ApplyEffect;
        onRemove = RemoveEffect;
    }

    protected override void ApplyEffect(Unit unit)
    {
        unit.defenseRate = 0.5f;
    }

    protected override void RemoveEffect(Unit unit)
    {
        unit.defenseRate = 1.0f;
    }
}
