using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScripts : MonoBehaviour
{
    private ApiGatewayManager apiGatewayManager;
    void Start(){
        UIAudioManager.instance.PlayBgm(true);
        apiGatewayManager = ApiGatewayManager.Instance;
    }
    // CLick button method
    public async void OnClickComponent(){
        await apiGatewayManager.StartMatch();
        StartCoroutine(MatchFinding());
        IEnumerator MatchFinding()
        {
            while (true)
            {
                if (GameManager.Instance._findingMatch == true)
                {
                    LoadingSceneController.LoadScene("GameScene_TEST");
                    break;
                }
                GameManager.Instance.OnFindMatchPressed();
                yield return new WaitForSeconds(3f);
            }
        }
    }
    public void OnClickSingle(){
        LoadingSceneController.LoadScene("GameScene_TEST");
    }
    public void OnClickExit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickSign_Out()
    {
        SceneManager.LoadScene("FirstScene");
        //apiGatewayManager.Logout();
    }


}
