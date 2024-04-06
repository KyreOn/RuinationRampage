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
        _effectSystem.AddEffect(new StunImmuneEffect(-1));
        _effectSystem.AddEffect(new UnstoppableEffect(-1));
    }

    public override bool OnCheckIsIdle()
    {
        return !(_giantSimpleAttack.isAttacking || _giantStrongAttack.isAttacking);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    
    protected override void OnStun()
    {
        _giantSimpleAttack.isAttacking = false;
        _giantStrongAttack.isAttacking = false;
    }
}
