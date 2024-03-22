using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float baseCooldown;
    [SerializeField] private int   maxCharges = 1;

    private float _effectedCooldown;
    private float _cooldownTimer;
    private int   _curCharges;
    
    protected bool isPreparing;

    public int level;
    
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
        _cooldownTimer = Mathf.Clamp(_cooldownTimer + Time.deltaTime, 0, _effectedCooldown);
        if (_cooldownTimer < _effectedCooldown) return;
        _curCharges++;
        _cooldownTimer = 0;
    }
}
