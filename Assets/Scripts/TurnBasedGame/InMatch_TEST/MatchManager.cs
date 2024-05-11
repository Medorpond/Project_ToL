using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchManager : MonoBehaviour
{
    public class ClickEvent: UnityEvent<GameObject> { }

    [Header("Click Event")]

    public ClickEvent onClickRelease = new ClickEvent();
    public ClickEvent onClickDown = new ClickEvent();
    

    [SerializeField]
    private PlayerManager player;
    [SerializeField]
    private PlayerManager opponent;

    public float unitSelectPhaseTime = 90f;
    public float battlePhaseTime = 60f;
    public int maxTurnCount = 100; // Draw if hit maxTurnCount
    private int currentTurnCount = 1;
    
    private void Awake()
    {
        SingletoneInit();
    }

    private void Start()
    {
        TimeManager.Instance.StartMatchTime();
        UnitSelectPhase();
    }
    private void Update()
    {
        GetClickRelease();
        GetClickDown();
    }

    private void OnDestroy()
    {
        if (instance == this) { instance = null; }
    }

    #region Singletone
    private static MatchManager instance = null;
    public static MatchManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MatchManager").AddComponent<MatchManager>();
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    #region Unit Select Phase
    void UnitSelectPhase()
    {
        Debug.Log("Unit Select Phase Start");
        
        TimeManager.Instance.onTimerEnd.AddListener(FinishDeploy);
        TimeManager.Instance.StartTimer(unitSelectPhaseTime);

        SetTurn();
    }

    void SetTurn()
    {
        int turn = Random.Range(0, 2); // Return 0 or 1, Player go First if 0.

        if (turn == 0) { player.isMyTurn = true; opponent.isMyTurn = false; }
        else { player.isMyTurn = false; opponent.isMyTurn = true; }

        Debug.Log($"{turn} Starts First.");
        // Need to have unified value over players, so should be done via server afterward
    }

    void FinishDeploy()
    {
        TimeManager.Instance.onTimerEnd?.RemoveListener(FinishDeploy);
        TimeManager.Instance.ResetTimer();
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();
        Debug.Log("Unit Select Phase End");
        BattlePhase();
    }
    #endregion

    #region BattlePhase

    void BattlePhase()
    {
        Debug.Log("Battle Phase Start");
        TimeManager.Instance.onTimerEnd.AddListener(ChangeTurn);
        TimeManager.Instance.StartTimer(battlePhaseTime);
    }

    public void ChangeTurn()
    {
        TimeManager.Instance.ResetTimer();

        currentTurnCount++;
        if (currentTurnCount > maxTurnCount)
        {
            GameOver();
            return;
        }

        player.isMyTurn = !player.isMyTurn;
        opponent.isMyTurn = !opponent.isMyTurn;

        // Disable all acts
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();

        TimeManager.Instance.StartTimer(battlePhaseTime);
    }

    public void GameOver()
    {
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();
        TimeManager.Instance.onTimerEnd?.RemoveListener(ChangeTurn);
        TimeManager.Instance?.ResetTimer();
        TimeManager.Instance?.EndMatchTime();
        Debug.Log("Game Over!");
        //trigger Result UI
    }
    #endregion

    #region GetClickMethod
    void GetClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider != null)
            {
                onClickDown?.Invoke(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("No Clickable Object");
            }
        }
    }

    void GetClickRelease()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider != null)
            {
                onClickRelease?.Invoke(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("No Clickable Object");
            }
        }
    }
    #endregion
}
