using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDodge : Spell
{
    [SerializeField] private float[] cooldown = new float[5];
    [SerializeField] private float[] speedBoost = new float[5];
    
    private CharacterController _controller;
    private EffectSystem        _effectSystem;
    private Animator            _animator;
    private DamageSystem        _damageSystem;
    private MovementSystem      _movementSystem;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        _damageSystem = GetComponent<DamageSystem>();
        _movementSystem = GetComponent<MovementSystem>();
    }

    protected override void OnPrepare()
    {
        baseCooldown = cooldown[level - 1];
        _animator.SetTrigger("Dodge");
        Cast();
    }
    
    protected override void OnCast()
    {
    }

    protected override void OnUpdate()
    {
        
    }
    
    public void DodgeStart()
    {
        _movementSystem.OnDodgeStart();
    }
    
    public void DodgeEnd()
    {
        _movementSystem.OnDodgeEnd(speedBoost[level-1]);
    }
}
