using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private Animator animator;

    public void Awake(){
        animator = GetComponent<Animator>();
    }
    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }
    private IEnumerator CloseAfterDelay(){
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
}
