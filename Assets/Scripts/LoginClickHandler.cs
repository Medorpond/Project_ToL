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
    
    //error

    [SerializeField]
    private TextMeshProUGUI Errortext;
    [SerializeField]
    private TextMeshProUGUI Errortext2;
    [SerializeField]
    private Image LoginFail;
    [SerializeField]
    private Image LoginFail2;
    [SerializeField]
    private Image LoginFail3;
    [SerializeField]
    private Image LoginFail4;
    [SerializeField]
    private Image LoginFail5;
    [SerializeField]
    private Image LoginFail6;

    // panel
    [SerializeField]
    private GameObject SignUppanel;
    [SerializeField]
    private GameObject EmailConfirmPanel;

    // Spinner
    [SerializeField]
    private GameObject LoadingSpinner1;
    [SerializeField]
    private GameObject LoadingSpinner2;

    // panel
    private void Start()
    {
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        ConfirmRegisterationButton.onClick.AddListener(Confirm);
        LoadingSpinner1.SetActive(false);
        LoadingSpinner2.SetActive(false);
    }
    
    private void Login()
    {
        LoadingSpinner1.SetActive(true);
        LoadingSpinner2.SetActive(true);
        apiGatewayManager.Login();
        //SceneManager.LoadScene("MainScene");
        StartCoroutine(WaitLoginStatus());
    }
    
    private void Register()
    {
        apiGatewayManager.Register();
        StartCoroutine(WaitRegisterStatus());
    }
    private void Confirm()
    {   
        string ConfirmCode = ConfirmCodeInputField.text;
        apiGatewayManager.ConfirmRegistration(ConfirmCode);
        SignUppanel.SetActive(false);
        EmailConfirmPanel.SetActive(false);
    }
    private IEnumerator WaitLoginStatus()
    {
        while(apiGatewayManager.isProgressIn())
        {
            yield return null;
        }

        LoadingSpinner1.SetActive(false);
        LoadingSpinner2.SetActive(false);

        if (apiGatewayManager.IsLoginsuccess())
        {
            Errortext.gameObject.SetActive(false);
            LoginFail.gameObject.SetActive(false);
            LoginFail2.gameObject.SetActive(false);
        }
        else
        {
            Errortext.gameObject.SetActive(true);
            LoginFail.gameObject.SetActive(true);
            LoginFail2.gameObject.SetActive(true);
        }
    }

    private IEnumerator WaitRegisterStatus()
    {
        while(apiGatewayManager.isProgressIn())
        {
            yield return null;
        }

        if (apiGatewayManager.IsRegistersuccess())
        {
            Errortext2.gameObject.SetActive(false);
            LoginFail3.gameObject.SetActive(false);
            LoginFail4.gameObject.SetActive(false);
            LoginFail5.gameObject.SetActive(false);
            LoginFail6.gameObject.SetActive(false);
            SignUppanel.gameObject.SetActive(false);
            EmailConfirmPanel.gameObject.SetActive(true);
        }
        else
        {
            Errortext2.gameObject.SetActive(true);
            LoginFail3.gameObject.SetActive(true);
            LoginFail4.gameObject.SetActive(true);
            LoginFail5.gameObject.SetActive(true);
            LoginFail6.gameObject.SetActive(true);
        }
    }
}
