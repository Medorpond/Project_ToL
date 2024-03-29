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

        // �÷��̾�� �� ���� �ʱ�ȭ
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
        // �÷��̾� ���϶��� ����
        if (state != State.playerTurn) return;
        else StartCoroutine("Attack");
    }

    private IEnumerator Attack()
    {
        // �� ����
        isEnemyLive = enemy.DecreaseHP(player.AttackDamage);

        // ���� ������ �̱�
        if (!isEnemyLive)
        {
            state = State.win;
            turn.text = "PLAYER Win";
            yield return new WaitForSeconds(3.0f);
            EndBattle();
        }
        // ���� ��������� ���� �ѱ�
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
        // player Turn �϶� ��
        if (state != State.playerTurn) return;
        player.IncreaseHP();

        // ���� �ѱ�
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
        yield return new WaitForSeconds(3.0f);

        isPlayerLive = player.DecreaseHP(enemy.AttackDamage);

        // �÷��̾ ���������
        if (isPlayerLive)
        {
            state = State.playerTurn;
            turn.text = "PLAYER Turn";
        }
        else
        {
            turn.text = "PLAYER Lose";
            yield return new WaitForSeconds(3.0f);
            EndBattle();
        }

        yield return null;
    }
}