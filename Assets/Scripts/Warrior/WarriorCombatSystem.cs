using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class WarriorCombatSystem : CombatSystem
{
    [SerializeField] private GameObject model;
    
    private Animator            _animator;
    private EffectSystem        _effectSystem;
    private bool                _isWeakAttacking;
    private bool                _isStrongAttacking;
    private CharacterController _controller;
    private WarriorWeakAttack   _warriorWeakAttack;
    private WarriorStrongAttack _warriorStrongAttack;
    private WarriorBlock        _warriorBlock;
    private WarriorSpellQ       _warriorSpellQ;
    private WarriorSpellE       _warriorSpellE;
    private WarriorSpellR        _warriorSpellR;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        _warriorWeakAttack = GetComponent<WarriorWeakAttack>();
        _warriorStrongAttack = GetComponent<WarriorStrongAttack>();
        _warriorBlock = GetComponent<WarriorBlock>();
        _warriorSpellQ = GetComponent<WarriorSpellQ>();
        _warriorSpellE = GetComponent<WarriorSpellE>();
        _warriorSpellR = GetComponent<WarriorSpellR>();
    }

    public override void StartWeak()
    {
        _warriorWeakAttack.Prepare();
    }
    
    public override void StopWeak()
    {
        _warriorWeakAttack.Cast();
    }

    public override void StartStrong()
    {
        _warriorStrongAttack.Prepare();
    }

    public override void StopStrong()
    {
        _warriorStrongAttack.Cast();
    }
    
    public override void PrepareDodge()
    {
        _warriorBlock.Prepare();
    }
    public override void CastDodge()
    {
        _warriorBlock.Cast();
    }

    public override void PrepareSpellQ()
    {
        _warriorSpellQ.Prepare();
    }
    public override void CastSpellQ()
    {
        _warriorSpellQ.Cast();
    }
    
    public override void PrepareSpellE()
    {
        _warriorSpellE.Prepare();
    }

    public override void CastSpellE()
    {
        _warriorSpellE.Cast();
    }
    
    public override void PrepareSpellR()
    {
        _warriorSpellR.Prepare();
    }

    public override void CastSpellR()
    {
        _warriorSpellR.Cast();
    }
}
