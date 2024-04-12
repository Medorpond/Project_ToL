using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource musicsource;
    //public AudioSource Btnsource;

    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }
}
