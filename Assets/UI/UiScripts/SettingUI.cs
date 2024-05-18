using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private Animator animator;

    public void Awake(){
        animator = GetComponent<Animator>();
        AudioManager.instance.EffectBgm(true);
    }
    public void Close()
    {
        AudioManager.instance.EffectBgm(false);
        StartCoroutine(CloseAfterDelay());
        
    }
    private IEnumerator CloseAfterDelay(){
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
}
