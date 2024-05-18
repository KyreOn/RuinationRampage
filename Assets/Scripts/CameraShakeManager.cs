using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    [SerializeField] private GameObject         cameraObject;

    private static float                              _timer;
    private static CinemachineVirtualCamera           _camera;
    private static CinemachineBasicMultiChannelPerlin _noise;
    public static  CameraShakeManager                 Instance { get; private set; }
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
        _camera = cameraObject.GetComponent<CinemachineVirtualCamera>();
        _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public static void ApplyNoise(float intensity, float time)
    {
        _noise.m_AmplitudeGain = intensity;
        _timer = time;
    }

    private void Update()
    {
        if (_timer <= 0) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
            _noise.m_AmplitudeGain = 0;
    }
}
