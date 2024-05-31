using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearInputField : MonoBehaviour
{
    private TMP_InputField[] inputFields;
    private void Awake()
    {
        inputFields = GetComponentsInChildren<TMP_InputField>();   
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable called");
        foreach (TMP_InputField inputField in inputFields)
        {
            if (inputField != null)
            {
                inputField.text = string.Empty;
            }
        }
    }
}
