using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSceneProfile : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI MainusernameText;
     [SerializeField]
    private TextMeshProUGUI MainwinText;
     [SerializeField]
    private TextMeshProUGUI MainloseText;
    [SerializeField]
    private TextMeshProUGUI usernameText;
    [SerializeField]
    private TextMeshProUGUI winText;
    [SerializeField]
    private TextMeshProUGUI loseText;
    [SerializeField]
    private TextMeshProUGUI subText;

    void Update()
    {
        UpdateUserInfo();
    }

    private void UpdateUserInfo()
    {
        ApiGatewayManager apiManager = ApiGatewayManager.Instance;

        if (apiManager != null)
        {
            MainusernameText.text = apiManager.__username;
            MainwinText.text = apiManager.__WIN;
            MainloseText.text = apiManager.__LOSE;
            usernameText.text = apiManager.__username;
            subText.text = apiManager.__SUB;
            winText.text = apiManager.__WIN;
            loseText.text = apiManager.__LOSE;
        }
    }
}
