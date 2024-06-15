using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class for_DeleteAccount_Input : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField Username;
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private Button DeleteButton;

    private ApiGatewayManager APIManager;

    private void Start()
    {
        APIManager = ApiGatewayManager.Instance;

        if (DeleteButton != null)
        {
            DeleteButton.onClick.AddListener(OnButtonDeleteAccount);
        }
    }

    private async void OnButtonDeleteAccount()
    {
        try
        {
            string _username = Username.text;
            string _password = password.text;

            //await APIManager.DeleteAccount();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Delete Account error: " + e.Message);
        }
    }

}
