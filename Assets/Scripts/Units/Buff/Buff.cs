using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.IO.LowLevel.Unsafe;

public class Buff
{
    public int persistTurn { get; private set; }

    protected Action<Unit> onApply;
    private Action<Unit> atTurnEnd;
    protected Action<Unit> onRemove;

    private Unit buffHolder;

    public Buff(int _persistTurn, Action<Unit> _onApply, Action<Unit> _atTurnEnd, Action<Unit> _onRemove, Unit _buffHolder)
    {
        persistTurn = _persistTurn;
        onApply = _onApply;
        atTurnEnd = _atTurnEnd;
        onRemove = _onRemove;
        buffHolder = _buffHolder;
    }

    public void Apply()
    {
        buffHolder.buffList.Add(this);
        //if (onApply != null) { onApply(buffHolder); }
        onApply?.Invoke(buffHolder);
    }

    public void TurnEnd()
    {
        //if(atTurnEnd != null) { atTurnEnd(buffHolder); }
        atTurnEnd?.Invoke(buffHolder);
        persistTurn--;
        if (persistTurn <= 0) { Remove(); }
    }

    public void Remove()
    {
        onRemove?.Invoke(buffHolder);

        if (buffHolder != null)
        {
            buffHolder.buffList.Remove(this);
        }

        onApply = null;
        atTurnEnd = null;
        onRemove = null;
        buffHolder = null;
    }

}
