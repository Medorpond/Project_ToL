using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private Coroutine cSetting;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && cSetting == null)
        {
            cSetting = StartCoroutine(ControlSettingUI());
        }
    }

    IEnumerator ControlSettingUI()
    {
        GameObject childObject = transform.Find("SettingsUI").gameObject;
        Animator animator = childObject.GetComponent<Animator>();
        if (childObject.activeSelf)
        {
            AudioManager.instance.EffectBgm(false);
            animator.SetTrigger("Close");
            yield return new WaitForSeconds(0.5f);
            childObject.SetActive(false);
            animator.ResetTrigger("Close");
        }
        else
        {
            AudioManager.instance.EffectBgm(true);
            childObject.SetActive(true);
            animator.SetTrigger("Open");
            animator.ResetTrigger("Open");
        }

        cSetting = null;
    }
}
