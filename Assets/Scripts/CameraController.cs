using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private bool                     _isDead;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        var offset = GameObject.Find("CameraOffset").transform;
        _camera.Follow = offset;
        _camera.LookAt = offset;
    }

    public void OnDeath()
    {
        _isDead = true;
    }

    private void Update()
    {
        if (!_isDead) return;
        _camera.m_Lens.OrthographicSize += Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale - 0.2f * Time.unscaledDeltaTime, 0, 1);
    }
}
