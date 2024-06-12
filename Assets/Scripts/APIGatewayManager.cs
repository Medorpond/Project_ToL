using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Runtime.CompilerServices;


public class ApiGatewayManager : MonoBehaviour
{
    #region Singletone
    private static ApiGatewayManager instance = null;
    public static ApiGatewayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("ApiGatewayManager").AddComponent<ApiGatewayManager>();
                //YONG
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    void Awake() {
        SingletoneInit();
    }

    public class LoginEvent : UnityEvent { }

    [Header("Login Events")]
    public LoginEvent onLoginSuccess = new LoginEvent();

    // yong
    public TMP_InputField emailInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPassWordInputField;

    public TMP_InputField LoginusernameInputField;
    public TMP_InputField LoginpasswordInputField;
    public TMP_InputField FindPass_UsernameInputField;

    public TMP_InputField NewPassInputField;
    public TMP_InputField NewPassReEnterInputField;

    private bool Loginsuccess = false;
    private bool ProgressIn = false;
    private bool Registersuccess = false;
    // yong

    private string _apiGatewayUrl = "https://zzjkwpmtzb.execute-api.ap-northeast-2.amazonaws.com/prod/";
    private string _username;
    private string _password;
    private string _email;
    private string _IdToken;
    private string _AccToken;
    private string _refreshToken;
    private string _ticketId;

    //Userdata, Username�� ���� �׸��� (_username)

    public string __username {get; private set;}
    public string __SUB {get; private set;}
    public string __WIN {get; private set;}
    public string __LOSE {get; private set;}



    // for debug
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("GO");
            GetUserInfo();
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
            
            string confirmPassword = confirmPassWordInputField.text;
            if (_password != confirmPassword)
            {
                Debug.LogError("Password and Confirm Password do not match");
                return;
            }
            ProgressIn = true;
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

                    //yong
                    Registersuccess = true;
                    ProgressIn = true;
                }
                else
                {
                    Debug.LogError("Registration failed. Status Code: " + response.StatusCode);

                    //yong
                    Registersuccess = false;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Registration error: " + e.Message);

            //yong
            Registersuccess = false;
        }
        //yong
        finally
        {
            ProgressIn = false;
        }
        //yong
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

            ProgressIn = true;
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
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(responseContent);
                    _IdToken = responseData["token"]["IdToken"];
                    _AccToken = responseData["token"]["AccessToken"];
                    _refreshToken = responseData["token"]["RefreshToken"];
                    onLoginSuccess.Invoke();
                    // Handle the response as needed

                    // yong
                    ProgressIn = true;
                    Loginsuccess = true;
                    await GetUserInfo();
                }
                else
                {
                    Debug.LogError("Login failed. Status Code: " + response.StatusCode);

                    //yong
                    Loginsuccess = false;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Login error: " + e.Message);

            // yong
            Loginsuccess = false;
        }
        //yong
        finally
        {
            ProgressIn = false;
        }
        //yong
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
            // yong
            _username = FindPass_UsernameInputField.text;
            // yong

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

    public async void ResetPassword (string confirmationCode)
    {
        try
        {
            //yong
            _password = NewPassInputField.text;
            //yong
            var requestData = new Dictionary<string, string>
            {
                { "confirmationCode", confirmationCode },
                { "NewPassword", _password},
                { "username", _username}
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiGatewayUrl + "confirm-forgot-password", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Reset Password successful.");
                }
                else
                {
                    Debug.LogError("Reset Password failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Reset Password error: " + e.Message);
        }
    }

    public async Task<bool> GetUserInfo()
    {
        try
        {
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", _IdToken);
                var response = await client.GetAsync(_apiGatewayUrl + "get-userinfo");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    Login();
                    return await GetUserInfo();
                }

                    if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonResponse);

                    var playerData = userInfo["playerData"];
                    __SUB = playerData["SUB"]["S"];
                    __username = playerData["username"]["S"];
                    __WIN = playerData["WIN"]["N"];
                    __LOSE = playerData["LOSE"]["N"];

                    Debug.Log("Get UserInfo Success");
                    //Debug.Log(jsonResponse);  //for debug
                    return true;
                }
                else
                {
                    Debug.LogError("Get UserInfo failed. Status Code: " + response.StatusCode);
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Get UserInfo error: " + e.Message);
            return false;
        }
    }

    public async Task<string> StartMatch()
    {
        try
        {
            var requestData = new Dictionary<string, Dictionary<string, int>>
            {
                { "latencyMap", new Dictionary<string, int> { { "ap-northeast-2", 60 } } }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", _IdToken);
                var response = await client.PostAsync(_apiGatewayUrl + "PollMatchmaking", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Login();
                    return await PollMatch(_ticketId);
                }

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    return jsonResponse;
                }
                else
                {
                    Debug.LogError("No Matchmaking Data. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Matchmaking error: " + e.Message);
        }
        return null;
    }

    public async Task<string> PollMatch(string _ticketId)
    {
        try
        {
            var requestData = new Dictionary<string, string>
            {
                { "ticketId", _ticketId}
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", _IdToken);
                var response = await client.PostAsync(_apiGatewayUrl + "PollMatchmaking", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Login();
                    return await PollMatch(_ticketId);
                }

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    return jsonResponse;
                }
                else
                {
                    Debug.Log("No Matchmaking Data. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Matchmaking error: " + e.Message);
        }
        return null;
    }

    public async void DeleteAccount()
    {
        try
        {
            //
            var requestData = new Dictionary<string, string>
            {
                { "username", _username },
                { "password", _password}
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", _IdToken);
                var response = await client.PostAsync(_apiGatewayUrl + "delete-account", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Login();
                    DeleteAccount();
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Account Deleted Successfully. Goob-bye");
                }
                else
                {
                    Debug.LogError("Delete Account failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Delete Account error: " + e.Message);
        }
    }

    public async void Logout()
    {
        try
        {
            //
            var requestData = new Dictionary<string, string>
            {
                { "AccessToken", _AccToken }
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", _IdToken);
                var response = await client.PostAsync(_apiGatewayUrl + "sign-out", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Login();
                    Logout();
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Log Out Successfully");
                }
                else
                {
                    Debug.LogError("Log Out failed. Status Code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Log Out error: " + e.Message);
        }
    }

    //yong
    public bool IsLoginsuccess()
    {
        return Loginsuccess;
    }
    public bool isProgressIn()
    {
        return ProgressIn;
    }
    public bool IsRegistersuccess()
    {
        return Registersuccess;
    }
    //yong

    
}