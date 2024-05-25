using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoginSceneScripts : MonoBehaviour
{

    public void Start(){
        UIAudioManager.instance.PlayBgm(true);
    }
    public void OnClickExit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
