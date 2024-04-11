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

public class TurnBasedGameManage : MonoBehaviour
{
    public State state;
    private bool isEnemyLive = true;
    private bool isPlayerLive = true;

    [SerializeField]
    private Player player;
    [SerializeField]
    private Player enemy;

    [SerializeField]
    private TextMeshProUGUI turn;

    // yong
    [SerializeField]
    private GameObject gameOverPanel;
    // yong



    private void Awake()
    {
        state = State.start;
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
        // get Enemy state
        isEnemyLive = enemy.TakeDamage(player.character.AttackDamage);

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
        Invoke("ShowGameOverPanel", 0f); // yong
    }

    private IEnumerator EnemyTurn()
    {
        turn.text = "ENEMY TURN";

        yield return new WaitForSeconds(1.5f);

        //get Player state
        isPlayerLive = player.TakeDamage(enemy.character.AttackDamage);

        // player Die
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
        SceneManager.LoadScene("MainScene");
    }
    // ~ yong
}
