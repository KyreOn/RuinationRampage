using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class ArcherCombatSystem : CombatSystem
{
    [SerializeField] private GameObject model;
    
    private Animator            _animator;
    private EffectSystem        _effectSystem;
    private bool                _isWeakAttacking;
    private bool                _isStrongAttacking;
    private CharacterController _controller;
    private ArcherWeakAttack    _archerWeakAttack;
    private ArcherStrongAttack  _archerStrongAttack;
    private ArcherDodge         _archerDodge;
    private ArcherSpellQ        _archerSpellQ;
    private ArcherSpellE        _archerSpellE;
    private ArcherSpellR        _archerSpellR;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        _archerWeakAttack = GetComponent<ArcherWeakAttack>();
        _archerStrongAttack = GetComponent<ArcherStrongAttack>();
        _archerDodge = GetComponent<ArcherDodge>();
        _archerSpellQ = GetComponent<ArcherSpellQ>();
        _archerSpellE = GetComponent<ArcherSpellE>();
        _archerSpellR = GetComponent<ArcherSpellR>();
    }

    public override void StartWeak()
    {
        _archerWeakAttack.Prepare();
    }
    
    public override void StopWeak()
    {
        _archerWeakAttack.Cast();
    }

    public override void StartStrong()
    {
        _archerStrongAttack.Prepare();
    }

    public override void StopStrong()
    {
        _archerStrongAttack.Cast();
    }
    
    public override void PrepareDodge()
    {
        _archerDodge.Prepare();
    }
    public override void CastDodge()
    {
        //_archerDodge.Cast();
    }

    public override void PrepareSpellQ()
    {
        _archerSpellQ.Prepare();
    }
    public override void CastSpellQ()
    {
        _archerSpellQ.Cast();
    }
    
    public override void PrepareSpellE()
    {
        _archerSpellE.Prepare();
    }

    public override void CastSpellE()
    {
        _archerSpellE.SetSpawnTransform(model.transform);
        _archerSpellE.Cast();
    }
    
    public override void PrepareSpellR()
    {
        _archerSpellR.Prepare();
    }

    public override void CastSpellR()
    {
        _archerSpellR.SetModel(model);
        _archerSpellR.Cast();
    }
}
