using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneProfile : MonoBehaviour
{
    
    [SerializeField]
    private TextMeshProUGUI Playername;
    [SerializeField]
    private TextMeshProUGUI Result_Playername;

    void Start()
    {
        UpdateName();    
    }

    private void UpdateName()
    {
        ApiGatewayManager apiManager = ApiGatewayManager.Instance;

        if (apiManager != null)
        {
            Playername.text = apiManager.__username;
            Result_Playername.text = apiManager.__username;
        }
         
    }
}
