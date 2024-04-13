using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBlock : Spell
{
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private float               _blockTimer;
    private bool                _isBlocking;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
    }
    
    protected override void OnPrepare()
    {
        _isBlocking = true;
        _blockTimer = 0;
        _animator.SetBool("Block",  true);
        _animator.SetBool("Slowed", true);
        _effectSystem.AddEffect(new SlowEffect(1, 2), false);
        _effectSystem.AddEffect(new ParryEffect(1, 1), false);
    }
    
    protected override void OnCast()
    {
        
    }

    protected override void OnUpdate()
    {
        if (!_isBlocking) return;
        _blockTimer += Time.deltaTime;
        if (_blockTimer >= 1)
        {
            _animator.SetBool("Block",  false);
            _animator.SetBool("Slowed", false);
            _isBlocking = false;
        }
    }
}
