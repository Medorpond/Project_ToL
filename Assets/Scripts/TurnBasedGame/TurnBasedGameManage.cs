using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum State
{
    start, playerTurn, enemyTurn, win, lose
}

public enum CharacterType
{
    king, tanker, dealer, healer
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

    [SerializeField]
    private TextMeshProUGUI turn;

    // yong
    [SerializeField]
    private GameObject gameOverPanel;
    bool time_active = true;   //timer active?
    public TextMeshProUGUI[] text_time; //timer text array
    float time;
    // yong



    private void Awake()
    {
        state = State.start;
        BeforeBattle();
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
    }

    private void BeforeBattle()
    {
        player.characters.Add(characters[(int)CharacterType.king]);
        enemy.characters.Add(characters[(int)CharacterType.dealer]);
        BattleStart();
    }

    private void BattleStart()
    {
        state = State.playerTurn;
        turn.text = "PLAYER Turn";
    }

    public void AttackButton()
    {
        // attack only at player Turn
        if (state != State.playerTurn) return;
        else StartCoroutine("Attack");
    }

    public void TurnEndButton()
    {
        // Change Player Turn to Enemy Turn
        if (state != State.playerTurn) return;
        else
        {
            state = State.enemyTurn;
            StartCoroutine("EnemyTurn");
        }
    }

    private IEnumerator Attack()
    {
        // can attack only at attackRange
        if (enemy.character.CanAttack(Vector2.Distance(enemy.character.location, player.character.location)))
        {
            // get Enemy state
            isEnemyLive = enemy.TakeDamage(player.character.AttackDamage);
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
        // player Turn
        if (state != State.playerTurn) return;
        player.character.Ability();
    }

    private void EndBattle()
    {
        turn.text = "BATTLE END";
        // yong
        Invoke("ShowGameOverPanel", 0f);
        time_active = false;
        // yong
    }

    private IEnumerator EnemyTurn()
    {
        turn.text = "ENEMY TURN";

        yield return new WaitForSeconds(1.5f);

        if (enemy.character.CanAttack(Vector2.Distance(enemy.character.location, player.character.location)))
            //get Player state
            isPlayerLive = player.TakeDamage(enemy.character.AttackDamage);

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
        }

        yield return null;
    }
    // yong ~
    void ShowGameOverPanel() {
        gameOverPanel.SetActive(true);
    }

    public void GoMain() {
        LoadingSceneController.LoadScene("MainScene");
    }
    // ~ yong

    // temp for select Character
    public void SelectKing()
    {
        if (player.character == null) player.SetCharacter(characters[(int)CharacterType.king]);
        else if (enemy.character == null) enemy.SetCharacter(characters[(int)CharacterType.king]);
        else return;
    }
    public void SelectHealer()
    {
        if (player.character == null) player.SetCharacter(characters[(int)CharacterType.healer]);
        else if (enemy.character == null) enemy.SetCharacter(characters[(int)CharacterType.healer]);
        else return;
    }
    public void SelectDealer()
    {
        if (player.character == null) player.SetCharacter(characters[(int)CharacterType.dealer]);
        else if (enemy.character == null) enemy.SetCharacter(characters[(int)CharacterType.dealer]);
        else return;
    }
    public void SelectTanker()
    {
        if (player.character == null) player.SetCharacter(characters[(int)CharacterType.tanker]);
        else if (enemy.character == null) enemy.SetCharacter(characters[(int)CharacterType.tanker]);
        else return;
    }
}
