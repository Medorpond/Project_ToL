using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SettingOpenESC : MonoBehaviour
{
    public GameObject SettingUI;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingUI != null)
            {
                SettingUI.SetActive(true);
            }
        }
    }
}
