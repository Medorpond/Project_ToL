using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.WSA;

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
    [SerializeField]
    private TextMeshProUGUI RateText;

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

            if(int.TryParse(apiManager.__WIN, out int wins) && int.TryParse(apiManager.__LOSE, out int losses))
            {
                if (wins + losses > 0)
                {
                    float winRate = (float) wins / (wins + losses);
                    RateText.text = (winRate * 100).ToString("F2") + "%";
                }
                else
                {
                    RateText.text = "0.00%";
                }
            }
            else
            {
                RateText.text = "N/A";
            }
        }
        else
        {
            Debug.LogError("no APIGatewayManager");
        }
    }
}
