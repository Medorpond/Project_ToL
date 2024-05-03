//using Amazon;
//using Amazon.CognitoIdentityProvider;
//using Amazon.CognitoIdentityProvider.Model;
//using Amazon.Extensions.CognitoAuthentication;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class CognitoAuth : MonoBehaviour
//{
//    private AmazonCognitoIdentityProviderClient providerClient;
//    private string userPoolId = "YOUR_USER_POOL_ID";
//    private string clientId = "YOUR_CLIENT_ID";
//    private string clientSecret = "YOUR_CLIENT_SECRET"; // Only if you have set it

//    void Start()
//    {
//        // Initialize the Amazon Cognito Identity Provider
//        providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.USEast1); // Use your actual region
//    }

//    public void SignUp(string username, string email, string password)
//    {
//        var signUpRequest = new SignUpRequest
//        {
//            ClientId = clientId,
//            SecretHash = GetSecretHash(username),
//            Username = username,
//            Password = password,
//            UserAttributes = new List<AttributeType>
//            {
//                new AttributeType
//                {
//                    Name = "email",
//                    Value = email
//                }
//            }
//        };

//        providerClient.SignUpAsync(signUpRequest, (response) =>
//        {
//            if (response.Exception == null)
//            {
//                Debug.Log("Sign up successful.");
//            }
//            else
//            {
//                Debug.LogError(response.Exception.Message);
//            }
//        });
//    }

//    public void SignIn(string username, string password)
//    {
//        var signInRequest = new InitiateAuthRequest
//        {
//            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
//            ClientId = clientId,
//            AuthParameters = new Dictionary<string, string>
//            {
//                { "USERNAME", username },
//                { "PASSWORD", password },
//                { "SECRET_HASH", GetSecretHash(username) }
//            }
//        };

//        providerClient.InitiateAuthAsync(signInRequest, (response) =>
//        {
//            if (response.Exception == null)
//            {
//                Debug.Log("Sign in successful.");
//            }
//            else
//            {
//                Debug.LogError(response.Exception.Message);
//            }
//        });
//    }

//    private string GetSecretHash(string username)
//    {
//        return new Amazon.Extensions.CognitoAuthentication.CognitoAuthHelper().GetSecretHash(username, clientId, clientSecret);
//    }
//}
