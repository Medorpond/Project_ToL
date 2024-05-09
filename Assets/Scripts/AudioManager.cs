using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;

    int channelIndex;

    public enum Sfx{sfx_click_ui, sfx_mouse_on_ui}

    void Awake(){
        instance = this;
        Init();
    }

    void Init(){
        //배경음 초기화
        GameObject bgmObject = new GameObject("BGM");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //SFX 초기화
        GameObject sfxObject = new GameObject("SFX");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0; index < channels;index++){
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].loop = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }
    public void mousePoint(){
        PlaySfx(Sfx.sfx_mouse_on_ui);
    }
    public void PlayClick(){
        PlaySfx(Sfx.sfx_click_ui);
    }
    
    public void PlaySfx(Sfx sfx){
        for(int index = 0; index < sfxPlayers.Length; index++){
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying){
                continue;
            }
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
        
    }
    public void PlayBgm(bool isPlay){
        if(isPlay){
            bgmPlayer.Play();
        }else{
            bgmPlayer.Stop();
        }
    }
}