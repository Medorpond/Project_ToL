using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public class TimeEvent: UnityEvent<float> { }

    [Header("Time Events")]
    public UnityEvent onTimerSet;
    public UnityEvent onTimerEnd;

    public TimeEvent onTimerTick;
    public TimeEvent onMatchTimeTick;

    private float timeLimit = 5f; // time Limit for each turn
    private float timeLeft;
    private float matchTime = 0f;
    private float gameTime = 0f;

    Coroutine timerCoroutine;
    Coroutine matchTimeCoroutine;
    Coroutine gameTimeCoroutine;

    private void Awake()
    {
        SingletoneInit();
        
    }
    private void Start()
    {
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

    #region Timer
    public void StartTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(SetTimerCoroutine());
            Debug.Log("Timer Set!");
            onTimerSet?.Invoke();
        }
        else
        {
            Debug.Log("에러! 에러! 에러!");
        }
    }
    IEnumerator SetTimerCoroutine()
    {
        Debug.Log("Timer start!");
        timeLeft = timeLimit;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            onTimerTick?.Invoke(timeLeft);
            yield return null;
        }

        if (timeLeft <= 0)
        {
            Debug.Log("Timeout!");
            ResetTimer();
            onTimerEnd?.Invoke();
        }
    }

    public void ResetTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            Debug.Log("Stop Timer");
        }        
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
            yield return null;
        }
    }

    IEnumerator MatchTimeCoroutine()
    {
        while (true)
        {
            matchTime += Time.deltaTime;
            onMatchTimeTick?.Invoke(matchTime);
            yield return null;
        }
    }
    #endregion
}
