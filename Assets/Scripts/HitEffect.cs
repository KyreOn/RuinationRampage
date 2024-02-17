using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private float      effectTime;
    
    private bool     _isHit;
    private float    _effectTimer;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = model.GetComponent<Renderer>();
        _renderer.material.SetInt("_Hit", 0);
    }

    public void ApplyDamage()
    {
        _isHit = true;
        _effectTimer = 0;
        _renderer.sharedMaterial.SetInt("_Hit", 1);
    }

    public void Update()
    {
        _effectTimer = Mathf.Clamp(_effectTimer + Time.deltaTime, 0, effectTime);
        if (_effectTimer >= effectTime && _isHit)
        {
            _isHit = false;
            _renderer.material.SetInt("_Hit", 0);
        }
    }
}
