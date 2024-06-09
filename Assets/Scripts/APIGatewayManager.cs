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

    private bool Loginsuccess = false;
    private bool ProgressIn = false;
    private bool Registersuccess = false;
    // yong

    private string _apiGatewayUrl = "https://zzjkwpmtzb.execute-api.ap-northeast-2.amazonaws.com/prod/";
    private string _username;
    private string _password;
    private string _email;
    private string _jwtToken;
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
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    _jwtToken = responseData["token"];
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
            var requestData = new Dictionary<string, string>
            {
                { "confirmationCode", confirmationCode },
                { "new_password", _password}
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

    public async Task GetUserInfo()
    {
        try
        {
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", _jwtToken); //Modified by Medorpond
                var response = await client.GetAsync(_apiGatewayUrl + "get-userinfo");

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

    public async Task<string> PollMatch(string _ticketId)
    {
        try
        {
            var requestData = new Dictionary<string, string>
            {
                { "ticketId", "a461374f-c2e3-4020-a7d9-8c3082264601"}
            };
            var json = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "eyJraWQiOiJIM28yYUp4dkNNdzhpd0VxSTZwTVc2SjJmXC9nM21VSDJKYmpNT1wvWTVzUHc9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJmNGY4NGRlYy0wMDcxLTcwYjMtZDJjMS1mMjVjY2Q4MGU1N2QiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLW5vcnRoZWFzdC0yLmFtYXpvbmF3cy5jb21cL2FwLW5vcnRoZWFzdC0yX1RxOXdZREhNdiIsImNvZ25pdG86dXNlcm5hbWUiOiJsankwMTIzIiwib3JpZ2luX2p0aSI6ImY5NWZkODYwLThlZTQtNDY1Ny1iZGRiLTY0Y2NlZThmZTk4MiIsImF1ZCI6IjQyaHE1bmYwaHZyZmpuaTF1Zmx0YjFiYW1kIiwiZXZlbnRfaWQiOiI5ZGEzYWRiMC1jYzU2LTRhODQtYTVkMy03NmQ1ZDlhZmVkZTAiLCJ0b2tlbl91c2UiOiJpZCIsImF1dGhfdGltZSI6MTcxNzkzNjE2NSwiZXhwIjoxNzE3OTM5NzY1LCJpYXQiOjE3MTc5MzYxNjUsImp0aSI6IjE2MDAzZjdiLWI5ZjktNDA1Mi05ZjNjLWQ3N2VmNDU4NTNiYiIsImVtYWlsIjoiY3FiMDcwOUBnbWFpbC5jb20ifQ.0u1zcFhCs63qqUOqfI0l96I-q_HUZ5vgoFzn1ukzHpjOeEz5HsrSqO2cHdazQOXMwXahHiX0jteiu0qBJxWWnEjrZS2UXILA-fitXBGSc-iHKMohZdEWh6TDwGQqgvxb2TnMzieHh2EprBEJW60c-LiHRFlB-VMDbCcknN5shFopktv0MYY-ZuUTmx8pDB5MK3Aqz4FKbl3al1B6Vl-AU_Gn4imvJRZ6oJLq99WucvztT6bzMxSUaaVFkAP4tgKp9CxsGXy8eh7bzS0L_0IoeskBJUg9aE0hErBqSZZMD1Bziy_eKw2SzM4G2sis-zdCsoMS7_2SHQPZ6IT4CbditA"); //Modified by Medorpond
                var response = await client.PostAsync(_apiGatewayUrl + "PollMatchmaking", content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    //var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                    
                    return jsonResponse;
                }
                else
                {
                    Debug.LogError("Matchmaking failed. Status Code: " + response.StatusCode);
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
                client.DefaultRequestHeaders.Add("Authorization", _jwtToken);

                var response = await client.PostAsync(_apiGatewayUrl + "delete-account", content);

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