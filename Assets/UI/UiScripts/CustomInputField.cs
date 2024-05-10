using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[RequireComponent(typeof(InputField))]
public class CustomInputField : MonoBehaviour
{
    private InputField inputField;
    private TextMeshProUGUI placeholderText;
    
    void Start()
    {
        inputField = GetComponent<InputField>();

        placeholderText = inputField.placeholder.GetComponent<TextMeshProUGUI>();

        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            placeholderText.enabled = true;
        }
        else
        {
            placeholderText.enabled = false;
        }
    }
}
