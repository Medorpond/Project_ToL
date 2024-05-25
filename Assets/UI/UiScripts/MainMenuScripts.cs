using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScripts : MonoBehaviour
{
    void Start(){
        UIAudioManager.instance.PlayBgm(true);
    }
    // CLick button method
    public void OnClickComponent(){
        LoadingSceneController.LoadScene("GameScene");
    }
    public void OnClickSingle(){
        SceneManager.LoadScene("GameScene_TEST");
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
