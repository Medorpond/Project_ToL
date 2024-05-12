using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CharacterType
{
    king, tanker, dealer, healer
}

public enum State
{
    start, playerTurn, enemyTurn, win, lose
}

public class TurnBasedGameManage : MonoBehaviour
{
    public State state;
    private bool isEnemyLive = true;
    private bool isPlayerLive = true;

    [SerializeField]
    private GameObject[] characters;

    [SerializeField]
    private Player player;
    [SerializeField]
    private Player enemy;

    private GameObject selectedUnit;
    private Character selectedCharacter;
    //private int unitIndex = -1;

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private TextMeshProUGUI turn;

    // yong
    [SerializeField]
    private GameObject gameOverPanel;
    bool time_active = true;   //timer active?
    public TextMeshProUGUI[] text_time; //timer text array
    float time;
    [SerializeField]
    private TextMeshProUGUI winner;
    public GameObject[] buttonPrefab;
    List<GameObject> controlbuttons = new List<GameObject>();
    // yong



    private void Awake()
    {
        state = State.start;
        BeforeBattle();
    }

    void Start(){
        AudioManager.instance.PlayBgm(true);
    }

    private void Update()
    {
        // yong timer text
        if (time_active) {
            time += Time.deltaTime;
            text_time[0].text = ((int)time / 60).ToString();
            text_time[1].text = ((int)time % 60).ToString();
        }
        // yong

        // check Clicked object only At player turn
        // before use enable tile collider
        if (state == State.playerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

                if (hit.transform == null) return;

                if (hit.transform.gameObject.tag == "Unit")
                {
                    if (selectedUnit == null || hit.transform.gameObject != selectedUnit)
                    {
                        selectedUnit = hit.transform.gameObject;
                        selectedCharacter = player.characters[player.GetIndex(selectedUnit)].GetComponentInChildren<Character>();

                        Debug.Log($"Unit Selected : {selectedUnit.name}");

                        cameraManager.ZoomCharacter(selectedCharacter.location);
                        //yong 
                        //ButtonCreate();
                        //yong
                    }

                    else
                    {
                        Debug.Log($"Unit unselected : {selectedUnit.name}");
                        selectedUnit = null;
                        selectedCharacter = null;
                        //yong 
                        //DestoryBtn();
                        //yong
                    }
                }
            }
        }
    }

    private void BeforeBattle()
    {
        // before battle

        BattleStart();
    }

    private void BattleStart()
    {
        state = State.playerTurn;
        turn.text = "PLAYER Turn";
    }

    public void AttackButton()
    {
        // attack only at player Turn, can attack only when chararcter selected
        if (state != State.playerTurn || selectedUnit == null) return;
        else StartCoroutine("Attack");

        //yong
        //DestoryBtn();
    }

    public void TurnEndButton()
    {
        // Change Player Turn to Enemy Turn
        if (state != State.playerTurn) return;
        else
        {
            // at enemy turn deactivate selected unit
            selectedUnit = null;
            selectedCharacter = null;
            cameraManager.ResetCamera();
            
            state = State.enemyTurn;
            StartCoroutine("EnemyTurn");
        }
    }

    private IEnumerator Attack()
    {
        // can attack only at attackRange
        if (selectedCharacter.CanAttack(Vector2.Distance(enemy.kingCharacter.location, selectedCharacter.location)))
        {
            // get Enemy state
            enemy.TakeDamage(selectedCharacter.AttackDamage);
            isEnemyLive = enemy.IsKingLive();
        }

        // enemy Die = Player Win
        if (!isEnemyLive)
        {
            state = State.win;
            turn.text = "PLAYER Win";
            yield return new WaitForSeconds(3.0f);
            EndBattle();
        }

        yield return null;
    }

    public void HealButton()
    {
        // didn't work
        // player Turn
        if (state != State.playerTurn) return;

        //yong
        //DestoryBtn();
    }

    public void MoveButton()
    {
        if (state != State.playerTurn || selectedUnit == null) return;
        selectedCharacter.canMove = true;

        // will add move turn count and move range

        //yong
        //DestoryBtn();
    }

    private void EndBattle()
    {
        turn.text = "BATTLE END";
        // yong
        Invoke("ShowGameOverPanel", 0f);
        time_active = false;
        // yong
    }

    // only attack my king
    private IEnumerator EnemyTurn()
    {
        turn.text = "ENEMY TURN";

        yield return new WaitForSeconds(1.5f);

        if (enemy.kingCharacter.CanAttack(Vector2.Distance(enemy.kingCharacter.location, player.kingCharacter.location)))
        //get Player state
        {
            player.TakeDamage(enemy.kingCharacter.AttackDamage);
            isPlayerLive = player.IsKingLive();
        }

        // player Live
        if (isPlayerLive)
        {
            state = State.playerTurn;
            turn.text = "PLAYER Turn";
        }
        else
        {
            turn.text = "PLAYER Lose";
            yield return new WaitForSeconds(1.5f);
            EndBattle();
           
            AudioManager.instance.PlayBgm(false);
        }

        cameraManager.ResetCamera();

        yield return null;
    }
    // yong ~
    void ShowGameOverPanel() {
        
        gameOverPanel.SetActive(true);
        Winner();
        AudioManager.instance.PlayBgm(false);
    }

    public void GoMain() {
        LoadingSceneController.LoadScene("MainScene");
    }

    private void Winner()
    {
        if(state == State.win)
        {
            winner.text = "Player win";
        }
        else
        {
            winner.text = "Enemy win";
        }
    }

    private void ButtonCreate()
    {
        Vector3 characterHeadPosition = selectedUnit.transform.position + Vector3.up * 2f;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterHeadPosition);

        GameObject canvas = GameObject.Find("Canvas");
    
        GameObject buttonATK = Instantiate(buttonPrefab[0], screenPosition + Vector3.up * 100f, Quaternion.identity);
        RectTransform buttonATKRect = buttonATK.GetComponent<RectTransform>();
        buttonATKRect.anchorMin = new Vector2(0f, 0f);
        buttonATKRect.anchorMax = new Vector2(0f, 0f);

        GameObject buttonMove = Instantiate(buttonPrefab[1], screenPosition + Vector3.up * 50f, Quaternion.identity);
        RectTransform buttonMoveRect = buttonMove.GetComponent<RectTransform>();
        buttonMoveRect.anchorMin = new Vector2(0f, 0f);
        buttonMoveRect.anchorMax = new Vector2(0f, 0f);
                        
        GameObject buttonAbility = Instantiate(buttonPrefab[2], screenPosition, Quaternion.identity);
        RectTransform buttonAbilityRect = buttonAbility.GetComponent<RectTransform>();
        buttonAbilityRect.anchorMin = new Vector2(0f, 0f);
        buttonAbilityRect.anchorMax = new Vector2(0f, 0f);

        buttonATK.transform.SetParent(canvas.transform, false);
        buttonMove.transform.SetParent(canvas.transform, false);
        buttonAbility.transform.SetParent(canvas.transform, false);
                        
        Button button = buttonATK.GetComponent<Button>();
        button.onClick.AddListener(AttackButton);
        Button button1 = buttonMove.GetComponent<Button>();
        button1.onClick.AddListener(MoveButton);
        Button button2 = buttonAbility.GetComponent<Button>();
        button2.onClick.AddListener(HealButton);

        controlbuttons.Add(buttonATK);
        controlbuttons.Add(buttonMove);
        controlbuttons.Add(buttonAbility);
    }

    private void DestoryBtn()
    {
        foreach(GameObject btn in controlbuttons)
            {
                Destroy(btn);
            }
    }
    // ~ yong
    // ȸ�� �ʿ�
}
