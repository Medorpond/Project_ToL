using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Amazon.CognitoIdentityProvider.Model;
using System;
using UnityEngine.SceneManagement;

public class LoginClickHandler : MonoBehaviour
{
    public Button LoginButton;
    public Button RegisterButton;
    public Button ConfirmRegisterationButton;
    public ApiGatewayManager apiGatewayManager;
    public TMP_InputField ConfirmCodeInputField;
    
    // panel
    [SerializeField]
    private GameObject SignUppanel;
    [SerializeField]
    private GameObject EmailConfirmPanel;

    // panel
    private void Start()
    {
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        ConfirmRegisterationButton.onClick.AddListener(Confirm);
    }
    
    private void Login()
    {
        apiGatewayManager.Login();
        //SceneManager.LoadScene("MainScene");
    }
    
    private void Register()
    {
        apiGatewayManager.Register();
        EmailConfirmPanel.SetActive(true);
    }
    private void Confirm()
    {   
        string ConfirmCode = ConfirmCodeInputField.text;
        apiGatewayManager.ConfirmRegistration(ConfirmCode);
        SignUppanel.SetActive(false);
        EmailConfirmPanel.SetActive(false);
    }
}
