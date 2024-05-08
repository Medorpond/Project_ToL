using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour, ISubject
{
    private float timeLimit = 60f; // time Limit for each turn
    private float timeLeft;
    private float matchTime = 0f;
    private float gameTime = 0f;

    Coroutine timerCoroutine;
    Coroutine matchTimeCoroutine;
    Coroutine gameTimeCoroutine;

    private void Awake()
    {
        SingletoneInit();
        StartGameTime();
    }

    #region Singletone
    private static TimeManager instance = null;
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("TimeManager").AddComponent<TimeManager>();
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    #region IObserver
    private List<IObserver> observerList = new List<IObserver>();
    public void RegisterObserver(IObserver observer)
    {
        observerList.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observerList.Remove(observer);
    }

    public void NotifyObservers(EventData data)
    {
        foreach (IObserver observer in observerList) { observer.Update(data); }
    }
    #endregion

    #region Timer
    public void StartTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(SetTimerCoroutine());
            NotifyObservers(new EventData { Type = EventType.Init }); // << Notify Observers
        }
    }
    IEnumerator SetTimerCoroutine()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            NotifyObservers(new EventData { Type = EventType.Event }); // << Notify Observers
            Debug.Log("Time Left... " + timeLeft);
            yield return null;
        }

        if (timeLeft <= 0)
        {
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
        timeLeft = timeLimit;
        NotifyObservers(new EventData { Type = EventType.End });
        Debug.Log("Timer Reset");
    }
    #endregion

    #region Elapsed Time
    public void StartGameTime()
    {
        if (gameTimeCoroutine == null)
        {
            gameTimeCoroutine = StartCoroutine(GameTimeCoroutine());
        }
    }
    public void StartMatchTime()
    {
        if (matchTimeCoroutine == null)
        {
            matchTimeCoroutine = StartCoroutine(MatchTimeCoroutine());
        }
    }

    public void EndMatchTime()
    {
        StopCoroutine(matchTimeCoroutine);
        matchTimeCoroutine = null;
        matchTime = 0f;
    }

    IEnumerator GameTimeCoroutine()
    {
        while (true)
        {
            gameTime += Time.deltaTime;
            Debug.Log("Elapsed GameTime: " + gameTime);
            yield return null;
        }
    }

    IEnumerator MatchTimeCoroutine()
    {
        while (true)
        {
            matchTime += Time.deltaTime;
            NotifyObservers(new EventData { Type = EventType.Event, NumericValue = timeLeft });
            Debug.Log("Elapsed MatchTime: " + matchTime);
            yield return null;
        }
    }
    #endregion
}
