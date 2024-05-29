using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class BattleAudioManager : MonoBehaviour
{
    public static BattleAudioManager instance;

    [Header("#SFX")]
    public AudioClip[] sfxClips; 
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;

    int channelIndex;

    [Header("#Ambience")]
    public AudioClip ambienceClip;
    public float ambienceVolume;
    AudioSource ambiencePlayer;
    



    public enum Sfx { damage, lightSwordAtk, sfx_shieldsUp = 7, victory1, victory2,doubleBladeAtk}
    private Dictionary<Unit.WeaponType, Sfx> weaponToSfxMap;

    public void Awake(){
        instance = this;
        Init();
        SetupWeaponToSfxMap();
    }

    public void Init()
    {
        GameObject ambienceObject = new GameObject("BGM");
        ambienceObject.transform.parent = transform;
        ambiencePlayer = ambienceObject.AddComponent<AudioSource>();
        ambiencePlayer.loop = true;
        ambiencePlayer.volume = ambienceVolume;
        ambiencePlayer.clip = ambienceClip;


        GameObject sfxObject = new GameObject("SFX");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < channels; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].loop = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }
    private void SetupWeaponToSfxMap()
    {
        weaponToSfxMap = new Dictionary<Unit.WeaponType, Sfx>
        {
            { Unit.WeaponType.LightSword, Sfx.lightSwordAtk },
            { Unit.WeaponType.Shield, Sfx.sfx_shieldsUp },
            { Unit.WeaponType.DoubleBlade, Sfx.doubleBladeAtk }
        };
    }

    public void PlayAmbience(bool isPlay)
    {
        if (isPlay)
        {
            ambiencePlayer.Play();
        }
        else
        {
            ambiencePlayer.Stop();
        }
    }

    public void PlayBSfx(Sfx sfx){
        for(int index = 0; index < sfxPlayers.Length; index++){
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
        public void PlayWeaponSfx(Unit.WeaponType weaponType)
    {
        if (weaponToSfxMap.TryGetValue(weaponType, out Sfx sfx))
        {
            PlayBSfx(sfx);
        }
    }
    
}