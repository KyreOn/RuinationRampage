using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : Enemy
{
    private EffectSystem      _effectSystem;
    private Animator          _animator;
    private GiantSimpleAttack _giantSimpleAttack;
    private GiantStrongAttack _giantStrongAttack;
    
    protected override void Awake()
    {
        base.Awake();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        _giantSimpleAttack = GetComponent<GiantSimpleAttack>();
        _giantStrongAttack = GetComponent<GiantStrongAttack>();
    }

    public override bool OnCheckIsIdle()
    {
        return !(_giantSimpleAttack.isAttacking || _giantStrongAttack.isAttacking);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
}
