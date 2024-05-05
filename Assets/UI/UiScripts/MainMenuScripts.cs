using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScripts : MonoBehaviour
{   
    // CLick button method
    public void OnClickComponent(){
        LoadingSceneController.LoadScene("GameScene");
    }
    public void OnClickSingle(){
        Debug.Log("SinglePlay");
    }
    public void OnClickExit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickSign_Out(){
        SceneManager.LoadScene("FirstScene");
    }
}
