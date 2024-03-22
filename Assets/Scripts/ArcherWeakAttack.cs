using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherWeakAttack : Spell
{
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
    }

    protected override void OnPrepare()
    {
        _animator.SetBool("isAttacking", true);
        _movementSystem.isAttacking = true;
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("isAttacking", false);
        _movementSystem.isAttacking = false;
    }

    protected override void OnUpdate()
    {
        
    }
}
