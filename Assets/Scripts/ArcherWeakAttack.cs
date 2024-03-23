using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherWeakAttack : Spell
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float[]    damage = new float[5];

    private CharacterController _controller;
    private EffectSystem        _effectSystem;
    private Animator            _animator;
    private MovementSystem      _movementSystem;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _effectSystem = GetComponent<EffectSystem>();
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
    
    public void Draw()
    {
        _controller.enabled = false;
        _animator.speed = 2.5f;
    }

    public void Shoot()
    {
        var arrow = Instantiate(arrowObject, spawnPoint.position, model.transform.rotation);
        arrow.GetComponent<Arrow>().Init(damage[level-1] * _effectSystem.CalculateOutcomeDamage());
    }
    
    public void ShootEnd()
    {
        _controller.enabled = true;
        _animator.speed = 1;
    }
}
