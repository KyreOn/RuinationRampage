using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[]  sfx;

    private static AudioSource  _source;
    private static AudioClip[]    _sfx;
    
    public static  AudioManager Instance { get; private set; }
    public static  int          currentWave;

    void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _sfx = sfx;
    }

    public static void PlaySFX(AudioClip sound)
    {
        _source.PlayOneShot(sound, (float)PlayerPrefs.GetInt("SFXVolume", 100) / 100);
    }
}
