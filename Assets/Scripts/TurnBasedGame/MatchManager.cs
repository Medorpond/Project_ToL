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

    private void Awake()
    {
        SingletoneInit();
    }

    private void Start()
    {
        TimeManager.Instance.StartMatchTime();
        TimeManager.Instance.onTimerEnd.AddListener(HandleTimerEnd);
        SetTurn();
        TimeManager.Instance.StartTimer();
    }
    private void Update()
    {
        GetClickRelease();
        GetClickDown();
    }

    private void OnDisable()
    {
            TimeManager.Instance.onTimerEnd?.RemoveListener(HandleTimerEnd);
            TimeManager.Instance?.EndMatchTime();
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



    void SetTurn()
    {
        int turn = Random.Range(0, 2); // Return 0 or 1, Player go First if 0.
        if(turn == 0) { player.isMyTurn = true; opponent.isMyTurn = false; }
        else { player.isMyTurn = false; opponent.isMyTurn = true; }
        Debug.Log($"{turn} Starts First.");
        // Need to have unified value over players, so should be done via server afterward
    }

    public void ChangeTurn()
    {
        TimeManager.Instance.ResetTimer();
        player.isMyTurn = !player.isMyTurn;
        opponent.isMyTurn = !opponent.isMyTurn;

        // Disable all acts
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();

        TimeManager.Instance.StartTimer();
    }
    private void HandleTimerEnd() => ChangeTurn();



    void GetClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray.z = 10;
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider != null)
            {
                onClickDown?.Invoke(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("No collided");
            }
        }
    }

    void GetClickRelease()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray.z = 10;
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider != null)
            {
                onClickRelease?.Invoke(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("No collided");
            }
        }
    }
}
