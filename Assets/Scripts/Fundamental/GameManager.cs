using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singletone
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if(instance == null) 
        { 
            instance = this; 
            DontDestroyOnLoad(this.gameObject); 
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    private void Awake()
    {
        SingletoneInit();
    }
}
