using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSceneProfile : MonoBehaviour
{
    public UserData userData;
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text winText;
    [SerializeField]
    private TMP_Text loseText;
    [SerializeField]
    private TMP_Text subText;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        usernameText.text = userData.username;
        winText.text = userData.WIN;
        loseText.text = userData.LOSE;
        subText.text = userData.SUB + "%";
    }
}
