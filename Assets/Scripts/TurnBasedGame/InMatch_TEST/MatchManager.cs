using NodeStruct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    private Phase currentPhase;
    
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

        currentPhase = Phase.UnitSelect;
        TimeManager.Instance.onTimerEnd.AddListener(FinishDeploy);
        TimeManager.Instance.StartTimer(unitSelectPhaseTime);

        SetTurn();
    }

    void SetTurn()
    {
        //int turn = Random.Range(0, 2); // Return 0 or 1, Player go First if 0.
        int turn = 0;
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

        DeployCaptain();

        BattlePhase();
    }

    void DeployCaptain()
    {
        Vector3 MyCaptainPos = MapManager.Instance.stage.PlayerLeaderPosition;
        Vector3 OpponentCaptainPos = MapManager.Instance.stage.OpponentLeaderPosition;

        string path = $"Prefabs/Character/Unit_TEST/Captain";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) { Debug.LogError("Failed to load prefab from path: " + path); return; }

        GameObject MyCaptain = Instantiate(prefab, MyCaptainPos, Quaternion.identity, player.transform);
        MapManager.Instance.stage.NodeArray[(int)MyCaptainPos.x, (int)MyCaptainPos.y].isBlocked = true;
        MyCaptain.GetComponent<BoxCollider2D>().enabled = true;
        MyCaptain.tag = "MyUnit";

        GameObject OpponentCaptain = Instantiate(prefab, OpponentCaptainPos, Quaternion.identity, opponent.transform);
        MapManager.Instance.stage.NodeArray[(int)OpponentCaptainPos.x, (int)OpponentCaptainPos.y].isBlocked = true;
        OpponentCaptain.GetComponent<BoxCollider2D>().enabled = true;
        OpponentCaptain.tag = "Opponent";
    }
    #endregion

    #region BattlePhase

    void BattlePhase()
    {
        Debug.Log("Battle Phase Start");
        
        currentPhase = Phase.Battle;

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
        currentPhase = Phase.End;
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

    public void DeployArcher()
    {
        if (currentPhase != Phase.UnitSelect) return;
        string path = $"Prefabs/Character/Unit_TEST/Archer";
        

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) { Debug.LogError("Failed to load prefab from path: " + path); return; }
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject unit = Instantiate(prefab,
            position,
            Quaternion.identity,player.transform);

        onClickDown?.Invoke(unit);

        StartCoroutine(DeployCoroutine());

        IEnumerator DeployCoroutine()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                int posX = (int)hit.collider.transform.position.x;
                int posY = (int)hit.collider.transform.position.y;
                Node node = MapManager.Instance.stage.NodeArray[posX, posY];

                if (!node.isBlocked && node.isDeployable)
                {
                    unit.transform.position = new Vector3(posX, posY);
                    MapManager.Instance.stage.NodeArray[posX, posY].isBlocked = true;
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                }
                else { Destroy(unit); }

            }
            else { Destroy(unit); }
        }
    }

    public void DeployKnight()
    {
        if (currentPhase != Phase.UnitSelect) return;
        string path = $"Prefabs/Character/Unit_TEST/Knight";

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) { Debug.LogError("Failed to load prefab from path: " + path); return; }
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject unit = Instantiate(prefab,
            position,
            Quaternion.identity, player.transform);

        onClickDown?.Invoke(unit);

        StartCoroutine(DeployCoroutine());

        IEnumerator DeployCoroutine()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                int posX = (int)hit.collider.transform.position.x;
                int posY = (int)hit.collider.transform.position.y;
                Node node = MapManager.Instance.stage.NodeArray[posX, posY];

                if (!node.isBlocked && node.isDeployable)
                {
                    unit.transform.position = new Vector3(posX, posY);
                    MapManager.Instance.stage.NodeArray[posX, posY].isBlocked = true;
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                }
                else { Destroy(unit); }

            }
            else { Destroy(unit); }
        }
    }

    public void DeployPriest()
    {
        if (currentPhase != Phase.UnitSelect) return;
        string path = $"Prefabs/Character/Unit_TEST/Priest";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) { Debug.LogError("Failed to load prefab from path: " + path); return; }
        GameObject unit = Instantiate(prefab,
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1),
            Quaternion.identity, player.transform);

        onClickDown?.Invoke(unit);

        StartCoroutine(DeployCoroutine());

        IEnumerator DeployCoroutine()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                int posX = (int)hit.collider.transform.position.x;
                int posY = (int)hit.collider.transform.position.y;
                Node node = MapManager.Instance.stage.NodeArray[posX, posY];

                if (!node.isBlocked && node.isDeployable)
                {
                    unit.transform.position = new Vector3(posX, posY);
                    MapManager.Instance.stage.NodeArray[posX, posY].isBlocked = true;
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                }
                else { Destroy(unit); }

            }
            else { Destroy(unit); }
        }
    }

    void GetClickDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider != null)
            {
                //onClickDown?.Invoke(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("No Clickable Object");
            }
        }
    }

    void GetClickRelease()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonUp(0) && currentPhase == Phase.Battle)
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


    private enum Phase
    {
        UnitSelect,
        Battle,
        End
    }
}
