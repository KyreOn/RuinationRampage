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

    private LevelSystem _levelSystem;
    private float       _effectedCooldown;
    private float       _cooldownTimer;
    private int         _curCharges;
    
    protected bool  isPreparing;
    
    public int    level;
    public bool   isUlt;
    
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
        _curCharges = maxCharges;
        _effectedCooldown = baseCooldown;
    }

    protected virtual void OnUpdate()
    {
        
    }
    protected virtual void Update()
    {
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
        _levelSystem.OnUpgrade();
    }
}
