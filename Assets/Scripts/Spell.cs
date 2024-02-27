using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float baseCooldown;
    [SerializeField] protected int   maxCharges;

    private   float _effectedCooldown;
    private   float _cooldownTimer;
    protected int   curCharges;
    protected bool  isPreparing;
    
    public void Prepare()
    {
        if (curCharges == 0) return;
        isPreparing = true;
        OnPrepare();
    }

    protected virtual void OnPrepare()
    {
        
    }
    
    public void Cast()
    {
        if (curCharges == 0 || !isPreparing) return;
        isPreparing = false;
        curCharges--;
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
        curCharges = maxCharges;
        _effectedCooldown = baseCooldown;
    }

    protected virtual void OnUpdate()
    {
        
    }
    protected virtual void Update()
    {
        OnUpdate();
        if (curCharges == maxCharges) return;
        _cooldownTimer = Mathf.Clamp(_cooldownTimer + Time.deltaTime, 0, _effectedCooldown);
        if (_cooldownTimer < _effectedCooldown) return;
        curCharges++;
        _cooldownTimer = 0;
    }
}
