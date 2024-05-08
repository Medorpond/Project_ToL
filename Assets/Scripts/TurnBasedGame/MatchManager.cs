using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    
    private void Awake()
    {
        TimeManager.Instance.StartMatchTime();
    }
    void SetTurn()
    {
        // here you set each player's turn...
    }

    void OnDestroy()
    {
        TimeManager.Instance.EndMatchTime();
    }
}
