using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager player;
    [SerializeField]
    private PlayerManager opponent;

    private void Start()
    {
        TimeManager.Instance.StartMatchTime();
        TimeManager.Instance.onTimerEnd.AddListener(HandleTimerEnd);
        SetTurn();
        TimeManager.Instance.StartTimer();
    }
    private void Update()
    {
        
    }

    void OnDisable()
    {
        TimeManager.Instance.onTimerEnd.RemoveListener(HandleTimerEnd);
        TimeManager.Instance.EndMatchTime();        
    }

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
        TimeManager.Instance.StartTimer();
        Debug.Log("Turn Changed!");
    }

    private void HandleTimerEnd() => ChangeTurn();
}
