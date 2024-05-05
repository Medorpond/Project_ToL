using UnityEngine;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;

public class CognitoManager : MonoBehaviour
{
    // Set your AWS region
    private string _region = "ap-northeast-2";

    // Set your user pool ID
    private string _userPoolId = "ap-northeast-2_Tq9wYDHMv";

    // Set your app client ID
    private string _clientId = "42hq5nf0hvrfjni1ufltb1bamd";

    // Set your username and password
    private string _username = "username";
    private string _password = "password";
    private string _email = "stcwh@naver.com";

    private string _token;

    private AmazonCognitoIdentityProviderClient _client;

    private void Start()
    {
        // Initialize the Cognito client
        _client = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.GetBySystemName(_region));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Try");
            SignIn();
        }
    }
    /*
    public async void SignUp() // 회원가입
    {
        SignUpRequest request = new SignUpRequest
        {
            ClientId = _clientId,
            Username = _username,
            Password = _password,
            UserAttributes = 
            {
                new AttributeType { Name = "email", Value = _email } 
            }
        };

        try
        {
            SignUpResponse response = await _client.SignUpAsync(request);
            Debug.Log("Sign up successful. Confirmation code sent: " + response.CodeDeliveryDetails.Destination);
        }
        catch (Exception e)
        {
            Debug.LogError("Sign up failed: " + e.Message);
        }
    }
    
    
    public async void ConfirmSignUp(string confirmationCode) // 이메일 인증 확인
    {
        ConfirmSignUpRequest request = new ConfirmSignUpRequest
        {
            ClientId = _clientId,
            Username = _username,
            ConfirmationCode = confirmationCode
        };

        try
        {
            ConfirmSignUpResponse response = await _client.ConfirmSignUpAsync(request);
            Debug.Log("Confirmation successful.");
        }
        catch (Exception e)
        {
            Debug.LogError("Confirmation failed: " + e.Message);
        }
    }
    */
    
    public async void SignIn() // 로그인
    {
        InitiateAuthRequest request = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH, //USER_PASSWORD_AUTH,
            ClientId = _clientId,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", _username },
                { "PASSWORD", _password }
            }
        };

        try
        {
            InitiateAuthResponse response = await _client.InitiateAuthAsync(request);
            _token = response.AuthenticationResult.AccessToken;
            Debug.Log("Sign in successful. Access token: " + _token);
        }
        catch (Exception e)
        {
            Debug.LogError("Sign in failed: " + e.Message);
        }
    }
}
