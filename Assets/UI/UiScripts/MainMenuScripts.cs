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
        GameManager.Instance._ticketId = await apiGatewayManager.StartMatch();
        StartCoroutine(MatchFinding());
        IEnumerator MatchFinding()
        {
            while (true)
            {
                GameManager.Instance.OnFindMatchPressed();
                if (GameManager.Instance._realTimeClient != null && GameManager.Instance._realTimeClient.GameStarted)
                {
                    LoadingSceneController.LoadScene("GameScene_TEST");
                    break;
                }
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
    public void OnClickSign_Out(){
        SceneManager.LoadScene("FirstScene");
    }
}
