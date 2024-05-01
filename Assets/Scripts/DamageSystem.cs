using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private float        effectTime;
    [SerializeField] private GameObject[] models;
    [SerializeField] public  float        health;
    [SerializeField] public  float[]      healthOnLevel = new float[28];

    private Animator         _animator;
    private CameraController _camera;
    private EffectSystem     _effectSystem;
    private bool             _isHit;
    private float            _effectTimer;
    private List<Renderer>   _renderers = new();
    private GameHUD          _gameHUD;
    
    public float curHealth;
    public float tempHealth;
    public bool  isInvincible;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        foreach (var model in models)
        {
            _renderers.Add(model.GetComponent<Renderer>());
        }
        _gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        if (gameObject.CompareTag("Player"))
        {
            health = healthOnLevel[0] * (PlayerPrefs.GetString($"ChosenPerks0").Contains('8') ? 0.8f : 1) * (PlayerPrefs.GetString($"ChosenPerks1").Contains('9') ? 1.2f : 1);
            _camera = FindObjectOfType<CameraController>();
            curHealth = health;
            tempHealth = _effectSystem.CalculateTempHealth();
            _gameHUD.UpdateHP(curHealth, tempHealth, health);
        }
        else
            curHealth = health;
    }

    public bool ApplyDamage(float damage, Transform source = null)
    {
        if (isInvincible) return false;
        if (_effectSystem.CheckForInvincibility()) return false;
        _animator.SetTrigger("Hit");
        if (_effectSystem.CanAssassinDodge() && source is not null && !_effectSystem.CheckIfRooted())
        {
            GetComponent<Reaction>().TryReact(source.gameObject);
            return false;
        }
        var parryEffect = _effectSystem.CheckForParry();
        if (parryEffect is not null && source is not null)
        {
            if (GetComponent<WarriorBlock>().CheckForParry(source))
            {
                if (source.CompareTag("Enemy") || source.CompareTag("Player"))
                {
                    if (PlayerPrefs.GetString($"ChosenPerks0").Contains('3'))
                        source.GetComponent<DamageSystem>().ApplyDamage(damage);
                    source.GetComponent<EffectSystem>().AddEffect(new StunEffect(parryEffect.ApplyEffect()));
                }
                if (source.CompareTag("Projectile") || source.CompareTag("Player"))
                {
                    
                }
                return false;
            }
        }
        _isHit = true;
        _effectTimer = 0;
        curHealth -= _effectSystem.DamageTempHealth(damage) * _effectSystem.CalculateIncomeDamage();
        tempHealth = _effectSystem.CalculateTempHealth();
        if (gameObject.CompareTag("Player"))
            _gameHUD.UpdateHP(curHealth, tempHealth, health);
        if (gameObject.CompareTag("Enemy"))
        {
            var hpBar = GetComponent<Enemy>().hpBar;
            if (hpBar is not null)
                hpBar.UpdateHP(curHealth, health);
        }
        
        if (curHealth <= 0)
        {
            if (gameObject.CompareTag("Enemy"))
                GetComponent<Enemy>().OnDeath();
            else
            {
                _camera.OnDeath();
                _gameHUD.OnDeath();
                WaveManager.currentWave = 0;
            }
                
        }
            
        foreach (var renderer in _renderers)
        {
            renderer.materials.Last().SetInt("_Hit", 1);
        }
        _effectSystem.AddEffect(new SlowEffect(0.2f, 1.5f));
        _effectSystem.AddEffect(new DamageEffect(0.2f, 1.25f), false);
        return true;
    }

    public bool ApplyHeal(float heal)
    {
        curHealth = Mathf.Clamp(curHealth + heal, 0, health);
        tempHealth = _effectSystem.CalculateTempHealth();
        if (gameObject.CompareTag("Player"))
            _gameHUD.UpdateHP(curHealth,  tempHealth, health);
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

    public void OnUpgrade(int level)
    {
        health = healthOnLevel[level]      * (PlayerPrefs.GetString($"ChosenPerks0").Contains('8') ? 0.8f : 1) * (PlayerPrefs.GetString($"ChosenPerks1").Contains('9') ? 1.2f : 1);
        curHealth *= healthOnLevel[level] / healthOnLevel[level - 1];
    }
}
