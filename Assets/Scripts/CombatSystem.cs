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
    private EffectSystem        _effectSystem;
    private bool                _isWeakAttacking;
    private bool                _isStrongAttacking;
    private CharacterController _controller;
    private ArcherWeakAttack    _archerWeakAttack;
    private ArcherStrongAttack  _archerStrongAttack;
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
    
    public void Draw()
    {
        _controller.enabled = false;
        _animator.speed = 2.5f;
    }
    public void Shoot()
    {
        var arrow = Instantiate(arrowObject, spawnPoint.position, model.transform.rotation);
        arrow.GetComponent<Arrow>().Init(20 * _effectSystem.CalculateOutcomeDamage());
    }

    public void ShootEnd()
    {
        _controller.enabled = true;
        _animator.speed = 1;
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
