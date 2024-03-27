using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Spell : MonoBehaviour
{
    [SerializeField] protected float  baseCooldown;
    [SerializeField] protected int    maxCharges = 1;
    [SerializeField] public    string title;
    [SerializeField] public    int    maxLevel;
    [SerializeField] protected int    id;
    [SerializeField] private   Sprite disabledSprite;
    [SerializeField] private   Sprite enabledSprite;
    
    private LevelSystem _levelSystem;
    private float       _effectedCooldown;
    private float       _cooldownTimer;
    private int         _curCharges;

    protected GameHUD gameHUD;
    protected bool    isPreparing;

    public int  level;
    public bool isUlt;

    public void Prepare()
    {
        if (_curCharges == 0 || level == 0) return;
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

    public void CalculateCooldown(float cdModifier)
    {
        _effectedCooldown = baseCooldown * cdModifier;
    }

    private void Start()
    {
        _levelSystem = GetComponent<LevelSystem>();
        gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        gameHUD.InitSkill(id, disabledSprite, enabledSprite, level);
        _curCharges = maxCharges;
        _effectedCooldown = baseCooldown;
    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void Update()
    {
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
