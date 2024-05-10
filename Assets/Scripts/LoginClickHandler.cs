using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class LoginClickHandler : MonoBehaviour
{
    public Button LoginButton;
    public Button RegisterButton;
    public Button ConfirmRegisterationButton;
    public ApiGatewayManager apiGatewayManager;
    string ConfirmCode;
    
    private void Start()
    {
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        ConfirmRegisterationButton.onClick.AddListener(Confirm);
    }
    private void Login()
    {
        apiGatewayManager.Login();
    }
    private void Register()
    {
        apiGatewayManager.Register();
    }
    private void Confirm()
    {
        apiGatewayManager.ConfirmRegistration(ConfirmCode);
    }
}
