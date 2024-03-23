using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject strongArrowObject;
    [SerializeField] private Transform  spawnPoint;
    
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

    public void StartWeak()
    {
        _archerWeakAttack.Prepare();
    }
    
    public void StopWeak()
    {
        _archerWeakAttack.Cast();
    }

    public void StartStrong()
    {
        _archerStrongAttack.Prepare();
    }

    public void StopStrong()
    {
        _archerStrongAttack.Cast();
    }
    
    public void PrepareDodge()
    {
        _archerDodge.Prepare();
    }
    public void CastDodge()
    {
        _archerDodge.Cast();
    }

    public void PrepareSpellQ()
    {
        _archerSpellQ.Prepare();
    }
    public void CastSpellQ()
    {
        //_archerSpellQ.StopPrepare();
        _archerSpellQ.Cast();
    }
    
    public void PrepareSpellE()
    {
        _archerSpellE.Prepare();
    }

    public void CastSpellE()
    {
        _archerSpellE.SetSpawnTransform(model.transform);
        _archerSpellE.Cast();
    }
    
    public void PrepareSpellR()
    {
        _archerSpellR.Prepare();
    }

    public void CastSpellR()
    {
        _archerSpellR.SetModel(model);
        _archerSpellR.Cast();
    }
}
