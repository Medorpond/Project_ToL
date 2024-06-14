using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_slider_sync : MonoBehaviour
{
    [SerializeField]
    private Button OnButton;
    [SerializeField]
    private Button OFFButton;
    public UIAudioManager Soundmanager;
    [SerializeField]
    private Slider soundslider;

    void Awake()
    {
        Soundmanager = GetComponent<UIAudioManager>();

        soundslider.onValueChanged.AddListener(OnSliderValueChanged);

        UpdateButtonState();
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateButtonState();
    }
    
    private void UpdateButtonState()
    {
        if (soundslider.value == 0)
        {
            OnButton.gameObject.SetActive(false);
            OFFButton.gameObject.SetActive(true);
            Soundmanager.TurnOffSound();
        }
        else
        {
            OnButton.gameObject.SetActive(true);
            OFFButton.gameObject.SetActive(false);
            Soundmanager.TurnOnSound();
        }
    }
}
