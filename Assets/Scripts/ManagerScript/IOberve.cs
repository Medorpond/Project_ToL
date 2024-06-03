using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IObserver { void Update(EventData data); }
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers(EventData data);
}

public class EventData
{
    public EventType Type { get; set; }
    public float NumericValue { get; set; }
    public string TextValue { get; set; }
    public bool BooleanValue { get; set; }
}

public enum EventType
{
    Event,
    Init,
    End
}
