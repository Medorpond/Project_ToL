using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class LoginClickHandler : MonoBehaviour
{
    public Button LoginButton;
    public Button RegisterButton;
    public Button ConfirmRegisterationButton;
    public Button ResendConfirmButton;
    public ApiGatewayManager apiGatewayManager;
    public TMP_InputField ConfirmCodeInputField;
    public TMP_InputField NewPassConfirmInputField;
    public Button ForgotButton;

    public Button ResetButton;
    
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
    private GameObject LoginPanel;
    [SerializeField]
    private GameObject SignUppanel;
    [SerializeField]
    private GameObject EmailConfirmPanel;
    [SerializeField]
    private GameObject Return_PassPanel;
    [SerializeField]
    private GameObject InputNewPassPanel;

    // Spinner
    [SerializeField]
    private GameObject LoadingSpinner1;
    [SerializeField]
    private GameObject LoadingSpinner2;

    // InputField to clear
    private TMP_InputField[] inputFieldsToClear;
    private TMP_InputField[] registerInputFieldsToClear;

    // for cooldown
    private int loginAttempts = 0;
    private bool isCooldown = false;
    private float cooldownTimer = 0f;
    private const int MAX_LOGIN_ATTEMPTS = 5;
    private const float COOLDOWN_DURATION = 30f;
    
    private void Start()
    {
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        ConfirmRegisterationButton.onClick.AddListener(Confirm);
        ResendConfirmButton.onClick.AddListener(Resend);
        ForgotButton.onClick.AddListener(Forgot);
        ResetButton.onClick.AddListener(Reset);
        LoadingSpinner1.SetActive(false);
        LoadingSpinner2.SetActive(false);

        if (LoginPanel != null)
        {
            inputFieldsToClear = LoginPanel.GetComponentsInChildren<TMP_InputField>();
        }
        if (SignUppanel != null)
        {
            registerInputFieldsToClear = SignUppanel.GetComponentsInChildren<TMP_InputField>();
        }
        
    }
    private void Update()
    {
        if(isCooldown)
        {
            CountdownCooldown();
        }
    }
    
    private void Login()
    {
        if (isCooldown)
        {
            return;
        }
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

    private void Resend()
    {
        apiGatewayManager.ResendConfirmation();
    }

    private void Forgot()
    {
        apiGatewayManager.ForgotPassword();
        Return_PassPanel.SetActive(false);
        InputNewPassPanel.SetActive(true);
    }
    
    private void Reset()
    {
        String ConfirmCode = NewPassConfirmInputField.text;
        apiGatewayManager.ResetPassword(ConfirmCode);
        InputNewPassPanel.SetActive(false);
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
            loginAttempts++;
            if (loginAttempts >= MAX_LOGIN_ATTEMPTS)
            {
                StartCooldown();
            }
            Errortext.gameObject.SetActive(true);
            LoginFail.gameObject.SetActive(true);
            LoginFail2.gameObject.SetActive(true);

            if(inputFieldsToClear != null)
            {
                foreach (TMP_InputField inputField in inputFieldsToClear)
                {
                    inputField.text = string.Empty;
                }
            }
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

            if(registerInputFieldsToClear != null)
            {
                foreach (TMP_InputField inputField in registerInputFieldsToClear)
                {
                    inputField.text = string.Empty;
                }
            }
        }
    }
    private void StartCooldown()
    {   
        isCooldown = true;
        cooldownTimer = COOLDOWN_DURATION;
        StartCoroutine(CountdownCooldown());
    }

    private IEnumerator CountdownCooldown()
    {
        while (cooldownTimer > 0f)
        {
            Errortext.text = "Too many login attempts. Please wait for " + cooldownTimer.ToString("F0") + " seconds.";
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        isCooldown = false;
        loginAttempts = 0;
        Errortext.text = "Invalid username or password";
        Errortext.gameObject.SetActive(false);
    }
}
