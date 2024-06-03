using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleWeather : Weather
{
    protected override void OnApply(Unit unit)
    {
        Debug.Log($"Weather applied to {unit.name}!");
    }

    protected override void OnTurnEnd(Unit unit)
    {
        Debug.Log($"Weather effected {unit.name} on turn End!");
    }

    protected override void OnRemove(Unit unit)
    {
        Debug.Log($"Weather removed from {unit.name}!");
    }
}
