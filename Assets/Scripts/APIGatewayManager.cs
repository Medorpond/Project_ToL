using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
public class ApiGatewayManager : MonoBehaviour
{
    // yong
    public TMP_InputField emailInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPassWordInputField;

    public TMP_InputField LoginusernameInputField;
    public TMP_InputField LoginpasswordInputField;
    // yong

    private string _apiGatewayUrl = "https://zzjkwpmtzb.execute-api.ap-northeast-2.amazonaws.com/prod/";
    private string _username;
    private string _password;
    private string _email;
    private string _jwtToken;


    /* for debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("GO");
            Login();
        }
    }
    */

    // Call this method to register a new user
    public async void Register()
    {
        try
        {
            // yong
            _email = emailInputField.text;
            _username = usernameInputField.text;
            _password = passwordInputField.text;
            // yong
            string confirmPassword = confirmPassWordInputField.text;
            if (_password != confirmPassword)
            {
                Debug.LogError("Password and Confirm Password do not match");
                return;
            }
            // yong 
            
            // Prepare the registration request payload
            var requestData = new Dictionary<string, string>
            {
                { "username", _username },
                { "password", _password },
                { "email", _email }
            };

            var json = JsonConvert.SerializeObject(requestData);
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the registration request to API Gateway
            using (var client = new HttpClient())
            {
                
                var response = await client.PostAsync(_apiGatewayUrl + "register", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Registration successful.");
                    // Proceed with confirmation if needed
                }
                else
                {
                    Debug.LogError("Registration failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Registration error: " + e.Message);
        }
    }
    
    // Call this method to confirm user registration with confirmation code
    public async void ConfirmRegistration(string confirmationCode)
    {
        try
        {
            // Prepare the confirmation request payload
            var requestData = new Dictionary<string, string>
            {
                { "username", _username },
                { "confirmationCode", confirmationCode }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the confirmation request to API Gateway
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiGatewayUrl + "confirm-registration", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Registration confirmation successful.");
                    // Proceed with login if needed
                }
                else
                {
                    Debug.LogError("Registration confirmation failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Registration confirmation error: " + e.Message);
        }
    }

    // Call this method to login
    public async void Login()
    {
        try
        {
            // yong
            _username = LoginusernameInputField.text;
            _password = LoginpasswordInputField.text;
            // yong

            // Prepare the login request payload
            var requestData = new Dictionary<string, string>
            {
                { "username", _username },
                { "password", _password }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the login request to API Gateway
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiGatewayUrl + "/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.Log("Login successful. Response: " + responseContent);

                    // Extract JWT token from response and store it
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    _jwtToken = responseData["token"];
                    // Handle the response as needed
                }
                else
                {
                    Debug.LogError("Login failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Login error: " + e.Message);
        }
    }

    public async void ResendConfirmation()
    {
        try
        {
            // Prepare the confirmation request payload
            var requestData = new Dictionary<string, string>
            {
                { "username", _username }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the confirmation request to API Gateway
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiGatewayUrl + "resend-confirmation", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Confirmation code resent successfully");
                    // Proceed with login if needed
                }
                else
                {
                    Debug.LogError("Confirmation code resent failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Confirmation code resent error: " + e.Message);
        }
    }

    public async void ForgotPassword()
    {
        try
        {
            // Prepare the confirmation request payload
            var requestData = new Dictionary<string, string>
            {
                { "username", _username }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the confirmation request to API Gateway
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiGatewayUrl + "forgot-password", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Password reset initiated successfully");
                    // Proceed with login if needed
                }
                else
                {
                    Debug.LogError("Password reset initiated failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Password reset initiated error: " + e.Message);
        }
    }

    public async void GetUserInfo()
    {
        try
        {
            //
            var requestData = new Dictionary<string, string>
            {
                { "token", _jwtToken }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiGatewayUrl + "get-userinfo", content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonResponse);

                    // 사용자 정보를 변수에 저장
                    string userId = userInfo.sub;

                    Debug.Log("Get UserInfo Success");
                    
                }
                else
                {
                    Debug.LogError("Get UserInfo failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Get UserInfo error: " + e.Message);
        }
    }
    
}

[System.Serializable]
public class UserInfo
{
    public string sub;
    // 여기에 다른 사용자 정보 필드를 추가

    public string GetSub()
    {
        return sub;
    }
}
