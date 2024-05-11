using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeUI;

    private void Start()
    {
        timeUI = gameObject.GetComponent<TextMeshProUGUI>();
        TimeManager.Instance.onTimerTick.AddListener(TimerUpdate);
    }


    void TimerUpdate(float _time)
    {
        timeUI.text = Mathf.Ceil(_time).ToString(); // This good
    }
}
