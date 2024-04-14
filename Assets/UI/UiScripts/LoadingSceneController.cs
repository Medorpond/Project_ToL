using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    [SerializeField]
    Image ProgressBar;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // Async for other process
        op.allowSceneActivation = false; // for fake loading = 1) too fast, 2) import error

        float timer = 0f;
        while(!op.isDone) // not Scene complete
        {
            yield return null;
            if(op.progress < 0.9f) // until 90%
            {
                ProgressBar.fillAmount = op.progress;
            }
            else // up 90%
            {
                timer += Time.unscaledDeltaTime;
                ProgressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 1seconds on 10%
                if(ProgressBar.fillAmount >= 1f)    // done
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
