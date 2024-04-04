using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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



    private void Awake()
    {
        state = State.start;
        BattleStart();

        // 플레이어와 적 정보 초기화
        player.Init();
        enemy.Init();
    }

    private void BattleStart()
    {
        state = State.playerTurn;
        turn.text = "PLAYER Turn";
    }

    public void AttackButton()
    {
        // 플레이어 턴일때만 공격
        if (state != State.playerTurn) return;
        else StartCoroutine("Attack");
    }

    private IEnumerator Attack()
    {
        // 적 공격
        isEnemyLive = enemy.DecreaseHP(player.AttackDamage);

        // 적이 죽으면 이김
        if (!isEnemyLive)
        {
            state = State.win;
            turn.text = "PLAYER Win";
            yield return new WaitForSeconds(3.0f);
            EndBattle();
        }
        // 적이 살아있으면 턴을 넘김
        else
        {
            state = State.enemyTurn;
            turn.text = "ENEMY Turn";
            StartCoroutine("EnemyTurn");
        }

        yield return null;
    }

    public void HealButton()
    {
        // player Turn 일때 힐
        if (state != State.playerTurn) return;
        player.IncreaseHP();

        // 턴을 넘김
        state = State.enemyTurn;
        turn.text = "ENEMY Turn";
        StartCoroutine("EnemyTurn");
    }

    private void EndBattle()
    {
        turn.text = "BATTLE END";
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);

        isPlayerLive = player.DecreaseHP(enemy.AttackDamage);

        // 플레이어가 살아있으면
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
}
