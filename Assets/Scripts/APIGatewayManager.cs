using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class ApiGatewayManager : MonoBehaviour
{
    private string _apiGatewayUrl = "https://zzjkwpmtzb.execute-api.ap-northeast-2.amazonaws.com/prod/";
    private string _username;
    private string _password;
    private string _email;
    

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
    
}
