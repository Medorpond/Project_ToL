using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips; 
    public AudioClip clickUiSound; 
    public AudioClip mouseOnUiSound; 
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;

    int channelIndex;

    // for volume Control
    public Slider volumeSlider;
    private float lastVolume;

    public enum Sfx { sfx_click_ui, sfx_mouse_on_ui }

    void Awake()
    {
        instance = this;
        if (volumeSlider != null) volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        Init();
    }

    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlaySfx(Sfx.sfx_click_ui));

            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { PlaySfx(Sfx.sfx_mouse_on_ui); });
            trigger.triggers.Add(entry);
        }
    }

    void Init()
    {
        // 배경음 초기화
        GameObject bgmObject = new GameObject("BGM");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

    
        if (volumeSlider != null) volumeSlider.value = bgmVolume;
        lastVolume = bgmVolume;

        // SFX 초기화
        GameObject sfxObject = new GameObject("SFX");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < channels; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].loop = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void mousePoint()
    {
        PlaySfx(Sfx.sfx_mouse_on_ui);
    }

    public void PlayClick()
    {
        PlaySfx(Sfx.sfx_click_ui);
    }

    public void PlaySfx(Sfx sfx)
    {
        AudioClip clipToPlay = null;
        switch (sfx)
        {
            case Sfx.sfx_click_ui:
                clipToPlay = clickUiSound;
                break;
            case Sfx.sfx_mouse_on_ui:
                clipToPlay = mouseOnUiSound;
                break;
        }

        if (clipToPlay != null)
        {
            AudioSource.PlayClipAtPoint(clipToPlay, Camera.main.transform.position, sfxVolume);
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    private void ChangeVolume()
    {
        bgmPlayer.volume = volumeSlider.value;
    }

    public void TurnOnSound()
    {
        bgmPlayer.volume = lastVolume;
        volumeSlider.value = bgmPlayer.volume;
    }

    public void TurnOffSound()
    {
        lastVolume = bgmPlayer.volume;
        bgmPlayer.volume = 0;
        volumeSlider.value = 0;
    }
}
