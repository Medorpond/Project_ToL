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

    [SerializeField]
    private GameObject[] characters;

    [SerializeField]
    private Player player;
    [SerializeField]
    private Player enemy;

    private GameObject selectedUnit;
    private Character selectedCharacter;
    private GameObject selectedEnemy;
    private Character enemyCharacter;

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
        BattleStart();
    }

    void Start()
    {
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
        
        // select Character
        if (state == State.playerTurn || state == State.enemyTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

                if (hit.transform == null) return;

                if (hit.transform.gameObject.tag == "Unit")
                {
                    // select
                    if (selectedUnit == null)
                    {
                        if (state == State.playerTurn)
                        {
                            if (isMyTeam(hit.transform.gameObject, player)) SelectCharacter(hit.transform.gameObject, player);
                        }
                        else if (state == State.enemyTurn)
                        {
                            if (isMyTeam(hit.transform.gameObject, enemy)) SelectCharacter(hit.transform.gameObject, enemy);
                        }
                    }

                    else
                    {
                        // select same unit, unselect
                        if (selectedUnit == hit.transform.gameObject)
                        {
                            Debug.Log($"Unit Unselected : {selectedUnit.name}");
                            selectedUnit = null;
                            selectedCharacter = null;
                            cameraManager.ResetCamera();
                        }
                        else
                        {
                            if (state == State.playerTurn)
                            {
                                // same team, reselect
                                if (isMyTeam(hit.transform.gameObject, player)) SelectCharacter(hit.transform.gameObject, player);
                                // another team, select enemy
                                else
                                {
                                    selectedEnemy = hit.transform.gameObject;
                                    enemyCharacter = selectedEnemy.GetComponentInChildren<Character>();
                                    Debug.Log($"Enemy Selected : {selectedEnemy.name}");
                                }
                            }
                            else if (state == State.enemyTurn)
                            {
                                // same team, reselect
                                if (isMyTeam(hit.transform.gameObject, enemy)) SelectCharacter(hit.transform.gameObject, enemy);
                                // another team, select enemy
                                else
                                {
                                    selectedEnemy = hit.transform.gameObject;
                                    enemyCharacter = selectedEnemy.GetComponentInChildren<Character>();
                                    Debug.Log($"Enemy Selected : {selectedEnemy.name}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void SelectCharacter(GameObject selected, Player user)
    {
        selectedUnit = selected;
        selectedCharacter = selectedUnit.GetComponentInChildren<Character>();
        Debug.Log($"Unit Selected : {user}.{selectedUnit.name}");

        cameraManager.ZoomCharacter(selectedCharacter.location);
    }

    private bool isMyTeam(GameObject unit, Player user)
    {
        // not in index, not my team
        if (user.GetIndex(unit) == -1) return false;
        else return true;
    }

    private void BattleStart()
    {
        state = State.playerTurn;
        ChangeStateText();
    }

    public void AttackButton()
    {
        // attack only at player Turn, can attack only when chararcter selected
        if (selectedUnit == null || selectedEnemy == null) return;
        else StartCoroutine("Attack");

        //yong
        //DestoryBtn();
    }

    public void TurnEndButton()
    {
        // deactivate selected unit
        selectedUnit = null;
        selectedCharacter = null;
        selectedEnemy = null;
        enemyCharacter = null;
        cameraManager.ResetCamera();

        // Change Player
        if (state == State.playerTurn) state = State.enemyTurn;
        else if (state == State.enemyTurn) state = State.playerTurn;
        ChangeStateText();
    }

    private IEnumerator Attack()
    {
        // can attack at attackRange
        if (selectedCharacter.CanAttack(Vector2.Distance(enemyCharacter.location, selectedCharacter.location)))
        {
            // attack
            selectedCharacter.Attack(enemyCharacter);
        }

        // check king state
        if (player.IsKingDead())
        {
            state = State.lose;
            ChangeStateText();
            yield return new WaitForSeconds(3.0f);
            EndBattle();
        }
        if (enemy.IsKingDead())
        {
            state = State.win;
            ChangeStateText();
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
        if (selectedUnit == null) return;
        selectedCharacter.canMove = true;

        // will add move turn count and move range

        //yong
        //DestoryBtn();
    }

    private void EndBattle()
    {
        // yong
        Invoke("ShowGameOverPanel", 0f);
        time_active = false;
        // yong
    }

    private void ChangeStateText()
    {
        switch (state)
        {
            case State.start: turn.text = "GAME START"; break;
            case State.playerTurn: turn.text = "PLAYER Turn"; break;
            case State.enemyTurn: turn.text = "ENEMY Turn"; break;
            case State.win: turn.text = "PLAYER Win"; break;
            case State.lose: turn.text = "ENEMY Win"; break;
            default: break;
        }
    }
    /*
    // only attack my king
    private IEnumerator EnemyTurn()
    {
        turn.text = "ENEMY TURN";

        yield return new WaitForSeconds(5.0f);

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
    */

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
