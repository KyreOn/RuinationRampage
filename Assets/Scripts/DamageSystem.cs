using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private float        effectTime;
    [SerializeField] private GameObject[] models;
    [SerializeField] public float        health;
    
    private EffectSystem   _effectSystem;
    private bool           _isHit;
    private float          _effectTimer;
    private List<Renderer> _renderers = new();
    private GameHUD        _gameHUD;
    private float          _curHealth;
    
    public bool  isInvincible;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        foreach (var model in models)
        {
            _renderers.Add(model.GetComponent<Renderer>());
        }
        _gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        _curHealth = health;
        if (gameObject.CompareTag("Player"))
            _gameHUD.UpdateHP(_curHealth, health);
    }

    public bool ApplyDamage(float damage)
    {
        if (isInvincible) return false;
        if (_effectSystem.CheckForInvincibility()) return false;
        _isHit = true;
        _effectTimer = 0;
        _curHealth -= damage * _effectSystem.CalculateIncomeDamage();
        if (gameObject.CompareTag("Player"))
            _gameHUD.UpdateHP(_curHealth, health);
        
        if (_curHealth <= 0)
        {
            if (gameObject.CompareTag("Enemy"))
                GetComponent<Enemy>().OnDeath();
        }
            
        foreach (var renderer in _renderers)
        {
            renderer.materials.Last().SetInt("_Hit", 1);
        }
        _effectSystem.AddEffect(new StunEffect(0.2f));
        _effectSystem.AddEffect(new DamageEffect(0.2f, 1.25f), false);
        return true;
    }

    public bool ApplyHeal(float heal)
    {
        _curHealth = Mathf.Clamp(_curHealth + heal, 0, health);
        if (gameObject.CompareTag("Player"))
            _gameHUD.UpdateHP(_curHealth, health);
        return true;
    }

    public void Update()
    {
        _effectTimer = Mathf.Clamp(_effectTimer + Time.deltaTime, 0, effectTime);
        if (_effectTimer >= effectTime && _isHit)
        {
            _isHit = false;
            foreach (var renderer in _renderers)
            {
                renderer.materials.Last().SetInt("_Hit", 0);
            }
        }

        var DOT = _effectSystem.CalculateDOT();
        if (DOT > 0)
            ApplyDamage(DOT);
    }

    public void SetInvincible(bool status)
    {
        isInvincible = status;
    }
}
