using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    private AudioSource _as;

    private void Awake()
    {
        _as = new AudioSource();
    }

    public void PlaySound(int i = 0)
    {
        _as.PlayOneShot(clips[i]);
    }
}
