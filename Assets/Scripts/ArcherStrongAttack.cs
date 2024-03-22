using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStrongAttack : Spell
{
    private Animator       _animator;
    private MovementSystem _movementSystem;
    private bool           _isStrongAttacking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
    }

    protected override void OnPrepare()
    {
        _isStrongAttacking = true;
        _animator.SetBool("isStrongAttack", _isStrongAttacking);
        _movementSystem.isAttacking = true;
    }
    
    protected override void OnCast()
    {
        _isStrongAttacking = false;
        _animator.SetBool("isStrongAttack", _isStrongAttacking);
        _movementSystem.isAttacking = false;
    }

    protected override void OnUpdate()
    {
        
    }
}
