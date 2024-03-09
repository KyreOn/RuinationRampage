using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private float      effectTime;
    
    private EffectSystem _effectSystem;
    private bool         _isHit;
    private bool         _isInvincible;
    private float        _effectTimer;
    private Renderer     _renderer;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _renderer = model.GetComponent<Renderer>();
        _renderer.material.SetInt("_Hit", 0);
    }

    public void ApplyDamage()
    {
        if (_isInvincible) return;
        _isHit = true;
        _effectTimer = 0;
        _renderer.materials[1].SetInt("_Hit", 1);
        _effectSystem.AddEffect(new DamageEffect(0.2f, 2), false);
    }

    public void Update()
    {
        _effectTimer = Mathf.Clamp(_effectTimer + Time.deltaTime, 0, effectTime);
        if (_effectTimer >= effectTime && _isHit)
        {
            _isHit = false;
            _renderer.materials[1].SetInt("_Hit", 0);
        }

        var DOT = _effectSystem.CalculateDOT();
        if (DOT > 0)
            ApplyDamage();
    }

    public void SetInvincible(bool status)
    {
        _isInvincible = status;
    }
}
