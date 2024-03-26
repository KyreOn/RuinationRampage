using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStrongAttack : Spell
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject strongArrowObject;
    [SerializeField] private Transform  spawnPoint;
    
    [SerializeField] private float[] damage        = new float[5];
    [SerializeField] private float[] bleedDuration = new float[5];
    [SerializeField] private float[] bleedDamage   = new float[5];
    [SerializeField] private int[]   pierceCount   = new int[5];
    
    private CharacterController _controller;
    private EffectSystem        _effectSystem;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private bool                _isStrongAttacking;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _effectSystem = GetComponent<EffectSystem>();
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
    
    public void StrongDraw()
    {
        _controller.enabled = false;
        _animator.speed = 1.2f;
    }
    
    public void StrongShoot()
    {
        var arrow = Instantiate(strongArrowObject, spawnPoint.position, model.transform.rotation);
        arrow.GetComponent<StrongArrow>().Init(damage[level-1] * _effectSystem.CalculateOutcomeDamage(), bleedDuration[level-1], bleedDamage[level-1], pierceCount[level-1]);
    }
    
    public void ShootEnd()
    {
        _controller.enabled = true;
        _animator.speed = 1;
    }

    public override string GetDescription()
    {
        var damageDiff   = damage[level]        - damage[level        - 1];
        var bleedDurDiff = bleedDuration[level] - bleedDuration[level - 1];
        var bleedDmgDiff = bleedDamage[level]   - bleedDamage[level   - 1];
        var pierceDiff   = (pierceCount[level] - pierceCount[level - 1]) == 0 ? "" : "Максимум целей: +1";
        return $"Урон: +{damageDiff}\nДлительность кровотечения: +{bleedDurDiff}с\nУрон за тик: +{bleedDmgDiff}\n{pierceDiff}";
    }
}
