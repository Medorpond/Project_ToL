using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldManager : MonoBehaviour
{
    public TMP_InputField inputID;
    public TMP_InputField inputPW;
    public CognitoManager cognitoManager;

    public void SendData()
    {
        if (cognitoManager != null)
        {
            cognitoManager.GetID(inputID.text);
            cognitoManager.GetPW(inputPW.text);
            cognitoManager.SignIn();
        }
    }
}
