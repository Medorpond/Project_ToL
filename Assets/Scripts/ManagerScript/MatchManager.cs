using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    #region Event System
    public class ClickEvent: UnityEvent<GameObject> { }
    public class EndTurnEvent: UnityEvent { }

    [Header("Click Event")]
    public ClickEvent onClickRelease = new ClickEvent();
    public ClickEvent onClickDown = new ClickEvent();
    public EndTurnEvent onTurnEnd = new EndTurnEvent();
    #endregion

    #region Serialized parameter

    public Button MainMenuBtn;
    public GameObject DeployPanel;
    public GameObject ResultPanel;

    //yong
    [SerializeField]
    private TextMeshProUGUI winnerText;
    //yong
    #endregion

    #region Parameter
    public float unitSelectPhaseTime = 90f;
    public float battlePhaseTime = 60f;
    public int maxTurnCount = 100; // Draw if hit maxTurnCount
    private int currentTurnCount = 1;

    private bool isDeploying = false;
    private Phase currentPhase;

    private PlayerManager player;
    private OpponentManager opponent;
    #endregion

    #region Unity Object LifeCycle
    private void Awake()
    {
        SingletoneInit();
        MainMenuBtn.onClick.AddListener(Exit);
        BattleAudioManager.instance.PlayAmbience(true);
    }
    private void Start()
    {
        player = PlayerManager.Instance;
        opponent = OpponentManager.Instance;
        UIAudioManager.instance.PlayBgm(true);
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
        if(player != null || opponent != null)
        {
            Destroy(player.gameObject);
            Destroy(opponent.gameObject);
        }
        if (instance == this) { instance = null; }
    }
    #endregion

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
        if (turn == 0) { player.isMyTurn = true; opponent.isMyTurn = false; Debug.Log($"{player} Starts First."); }
        else { player.isMyTurn = false; opponent.isMyTurn = true; Debug.Log($"{opponent} Starts First."); }

        // Need to have unified value over players, so should be done via server afterward
    }

    void FinishDeploy()
    {
        DeployCaptain();
        opponent.CreateSampleSet();
        TimeManager.Instance.onTimerEnd?.RemoveListener(FinishDeploy);
        TimeManager.Instance.ResetTimer();
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();
        DeployPanel.SetActive(false);
        Debug.Log("Unit Select Phase End");

        StartCoroutine(nameof(BattlePhase));
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
        MapManager.Instance.stage.NodeArray[(int)MyCaptainPos.x, (int)MyCaptainPos.y].unitOn = MyCaptain.GetComponent<Unit>();
        MyCaptain.GetComponent<BoxCollider2D>().enabled = true;
        player.RegisterUnit(MyCaptain.GetComponent<Unit>());

        GameObject OpponentCaptain = Instantiate(prefab, OpponentCaptainPos, Quaternion.identity, opponent.transform);
        MapManager.Instance.stage.NodeArray[(int)OpponentCaptainPos.x, (int)OpponentCaptainPos.y].isBlocked = true;
        MapManager.Instance.stage.NodeArray[(int)OpponentCaptainPos.x, (int)OpponentCaptainPos.y].unitOn = OpponentCaptain.GetComponent<Unit>();
        OpponentCaptain.GetComponent<BoxCollider2D>().enabled = true;
        opponent.RegisterUnit(OpponentCaptain.GetComponent<Unit>());
    }
    #endregion

    #region BattlePhase

    IEnumerator BattlePhase()
    {
        Debug.Log("Battle Phase Start");   
        currentPhase = Phase.Battle;
        TimeManager.Instance.onTimerEnd.AddListener(ChangeTurn);

        yield return new WaitUntil(() => isDeploying == false);

        
        TimeManager.Instance.StartTimer(battlePhaseTime);
        //MapManager.Instance.InitWeather(player, opponent);
    }

    public void ChangeTurn()
    {
        TimeManager.Instance.ResetTimer();

        currentTurnCount++;
        if (currentTurnCount > maxTurnCount)
        {
            GameOver(null);
            return;
        }

        player.EndingTurn();
        player.isMyTurn = !player.isMyTurn;
        opponent.isMyTurn = !opponent.isMyTurn;

        // Disable all acts
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();

        foreach (string elem in player.CmdList)
        {
            //Debug.Log($"{elem}");
        }
        // Send player CmdList via Network HERE.

        player.CmdList.Clear();

        onTurnEnd.Invoke();
        TimeManager.Instance.StartTimer(battlePhaseTime);
    }
    #endregion

    #region EndPhase
    public void GameOver(GameObject loser = null)
    {
        BattleAudioManager.instance.PlayAmbience(false);
        UIAudioManager.instance.PlayBgm(false);
        currentPhase = Phase.End;
        player.StopAllCoroutines();
        opponent.StopAllCoroutines();
        TimeManager.Instance.onTimerEnd?.RemoveListener(ChangeTurn);
        TimeManager.Instance?.ResetTimer();
        TimeManager.Instance?.EndMatchTime();
        //trigger Result UI with LoserData. if null, it's a draw
        if (loser.CompareTag("Opponent"))
        {
            winnerText.text = "You Win!!!";
            BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.victory1);
            BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.victory2);
        }
        else if (loser.CompareTag("Player"))
        {
            winnerText.text = "You Lose...";
            BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.lose);
            //BattleAudioManager.instance.PlayBSfx(BattleAudioManager.Sfx.lose2);
        }
        else
        {
            //Draw
        }
        ResultPanel.SetActive(true);

        
    }
    #endregion

    #region GetClickMethod

    public void DeployArcher()
    {
        DeployUnit("Prefabs/Character/Unit_TEST/Archer");
    }

    public void DeployKnight()
    {
        DeployUnit("Prefabs/Character/Unit_TEST/Knight");
    }

    public void DeployAssassin(){
        DeployUnit("Prefabs/Character/Unit_TEST/Priest");
    }
    
    public void DeployPriest()
    {
        DeployUnit("Prefabs/Character/Unit_TEST/Priest");
    }

    public void DeployMagician()
    {
        DeployUnit("Prefabs/Character/Unit_TEST/Magician");
    }

    public void DeployAxeGiant()
    {
        DeployUnit("Prefabs/Character/Unit_TEST/AxeGiant");
    }

    public void DeployShield()
    {
        DeployUnit("Prefabs/Character/Unit_TEST/Shield");
    }


    public void DeployUnit(string path)
    {
        if (currentPhase != Phase.UnitSelect) return;

        isDeploying = true;

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) { Debug.LogError("Failed to load prefab from path: " + path); return; }
        GameObject unit = Instantiate(prefab,
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
            Quaternion.identity, player.transform);

        onClickDown?.Invoke(unit);
        UIAudioManager.instance.PlaySfx(UIAudioManager.Sfx.sfx_grab_unit);

        DeployPanel.SetActive(false);

        StartCoroutine(DeployCoroutine());

        IEnumerator DeployCoroutine()
        {
            yield return new WaitUntil(() => currentPhase == Phase.Battle || Input.GetMouseButtonUp(0));
            UIAudioManager.instance.PlaySfx(UIAudioManager.Sfx.sfx_deploy_unit);

            RaycastHit2D hit = RayCast();
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                int posX = (int)hit.collider.transform.position.x;
                int posY = (int)hit.collider.transform.position.y;
                Node node = MapManager.Instance.stage.NodeArray[posX, posY];

                if (!node.isBlocked && node.isDeployable)
                {
                    unit.transform.position = new Vector3(posX, posY);
                    MapManager.Instance.stage.NodeArray[posX, posY].isBlocked = true;
                    MapManager.Instance.stage.NodeArray[posX, posY].unitOn = unit.GetComponent<Unit>();
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                    player.RegisterUnit(unit.GetComponent<Unit>());
                    if (currentPhase == Phase.UnitSelect)
                    {
                        DeployPanel.SetActive(true);
                    }
                }
                else
                {
                    Destroy(unit);
                    if (currentPhase == Phase.UnitSelect)
                    {
                        DeployPanel.SetActive(true);
                    }
                }

            }
            else
            {
                Destroy(unit);
                if (currentPhase == Phase.UnitSelect)
                {
                    DeployPanel.SetActive(true);
                }
            }
            isDeploying = false;
        }
    }


    void GetClickDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = RayCast();

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
            RaycastHit2D hit = RayCast();

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

    void Exit()
    {
        SceneManager.LoadScene("MainScene");
    }
    private enum Phase
    {
        UnitSelect,
        Battle,
        End
    }

    public RaycastHit2D RayCast()
    {
        int layerMask = ~LayerMask.GetMask("Ignore Raycast");
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.Raycast(ray, Vector2.zero);
    }
}
