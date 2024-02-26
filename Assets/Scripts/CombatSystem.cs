using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private GameObject strongArrowObject;
    [SerializeField] private Transform  spawnPoint;
    
    private Animator            _animator;
    private bool                _isWeakAttacking;
    private bool                _isStrongAttacking;
    private CharacterController _controller;
    private MovementSystem      _movementSystem;
    private ArcherSpellQ        _archerSpellQ;
    private ArcherSpellE        _archerSpellE;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
        _archerSpellQ = GetComponent<ArcherSpellQ>();
        _archerSpellE = GetComponent<ArcherSpellE>();
    }

    public void StartWeak()
    {
        _isWeakAttacking = true;
        _animator.SetBool("isAttacking", _isWeakAttacking);
        _movementSystem.isAttacking = true;
    }
    
    public void StopWeak()
    {
        _isWeakAttacking = false;
        _animator.SetBool("isAttacking", _isWeakAttacking);
        _movementSystem.isAttacking = false;
    }

    public void StartStrong()
    {
        _isStrongAttacking = true;
        _animator.SetBool("isStrongAttack", _isStrongAttacking);
        _movementSystem.isAttacking = true;
    }

    public void StopStrong()
    {
        _isStrongAttacking = false;
        _animator.SetBool("isStrongAttack", _isStrongAttacking);
        _movementSystem.isAttacking = false;
    }

    public void Weak()
    {
        _isWeakAttacking = !_isWeakAttacking;
        _animator.SetBool("isAttacking", _isWeakAttacking);
    }

    public void Draw()
    {
        _controller.enabled = false;
        _animator.speed = 2.5f;
    }
    public void Shoot()
    {
        Instantiate(arrowObject, spawnPoint.position, model.transform.rotation);
    }

    public void ShootEnd()
    {
        _controller.enabled = true;
        _animator.speed = 1;
    }

    private void Update()
    {
        
    }

    public void Strong()
    {
        _isWeakAttacking = !_isWeakAttacking;
        _animator.SetBool("isStrongAttack", _isWeakAttacking);
    }

    public void StrongDraw()
    {
        _controller.enabled = false;
        _animator.speed = 1.2f;
    }
    
    public void StrongShoot()
    {
        Instantiate(strongArrowObject, spawnPoint.position, model.transform.rotation);
    }

    public void PrepareSpellQ()
    {
        _archerSpellQ.Prepare();
    }
    public void CastSpellQ()
    {
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
        
    }

    public void CastSpellR()
    {
        
    }
}
