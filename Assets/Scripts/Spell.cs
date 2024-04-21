using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Spell : MonoBehaviour
{
    [SerializeField] protected float  baseCooldown;
    [SerializeField] protected int    maxCharges = 1;
    [SerializeField] public    string title;
    [SerializeField] public    int    maxLevel;
    [SerializeField] public    int    id;
    [SerializeField] public    Sprite disabledSprite;
    [SerializeField] public    Sprite enabledSprite;
    
    private   LevelSystem  _levelSystem;
    private float        _effectedCooldown;
    protected   float        _cooldownTimer;
    private   int          _curCharges;
    protected   EffectSystem _effectSystem;

    protected GameHUD gameHUD;
    protected bool    isPreparing;
    protected bool    isBlocked;

    public int  level;
    public bool isUlt;

    public void Prepare()
    {
        if (_curCharges == 0 || level == 0 || isBlocked || _effectSystem.CheckIfStunned()) return;
        isPreparing = true;
        OnPrepare();
    }

    protected virtual void OnPrepare()
    {

    }

    public void Cast()
    {
        if (_curCharges == 0 || !isPreparing) return;
        isPreparing = false;
        _curCharges--;
        OnCast();
    }

    protected virtual void OnCast()
    {

    }

    public void Use()
    {
        
    }

    public void CalculateCooldown(float cdModifier)
    {
        _effectedCooldown = baseCooldown * cdModifier;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        _levelSystem = GetComponent<LevelSystem>();
        gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        gameHUD.InitSkill(id, disabledSprite, enabledSprite, level);
        _curCharges = maxCharges;
        _effectedCooldown = baseCooldown;
        _effectSystem = GetComponent<EffectSystem>();
    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        gameHUD.UpdateSkill(id, _cooldownTimer, baseCooldown, _curCharges, maxCharges);
        OnUpdate();
        if (_curCharges == maxCharges) return;
        _cooldownTimer = Mathf.Clamp(_cooldownTimer + Time.deltaTime, 0, baseCooldown);
        if (_cooldownTimer < baseCooldown) return;
        _curCharges++;
        _cooldownTimer = 0;
    }

    public virtual string GetDescription()
    {
        return "";
    }

    public void Upgrade()
    {
        level++;
        OnUpgrade();
        gameHUD.InitSkill(id, disabledSprite, enabledSprite, level);
        _levelSystem.OnUpgrade();
    }

    protected virtual void OnUpgrade()
    {
        
    }

}
