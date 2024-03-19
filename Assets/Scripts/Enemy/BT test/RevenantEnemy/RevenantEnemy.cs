using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantEnemy : Enemy
{
    private EffectSystem         _effectSystem;
    private DamageSystem         _damageSystem;
    private Animator             _animator;
    private RevenantSimpleAttack _revenantSimpleAttack;
    private RevenantStrongAttack _revenantStrongAttack;
    private float                _reviveTimer;
    
    
    public bool revived;
    public bool toRevive;
    
    
    protected override void Awake()
    {
        base.Awake();
        _effectSystem = GetComponent<EffectSystem>();
        _damageSystem = GetComponent<DamageSystem>();
        _animator = GetComponent<Animator>();
        _revenantSimpleAttack = GetComponent<RevenantSimpleAttack>();
        _revenantStrongAttack = GetComponent<RevenantStrongAttack>();
    }

    public override bool OnCheckIsIdle()
    {
        return !(_revenantSimpleAttack.isAttacking || _revenantStrongAttack.isAttacking);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _reviveTimer += Time.deltaTime;
        if (_damageSystem.health <= 60)
            toRevive = true;
        
        if (revived) return;
        if (toRevive)
        {
            revived = true;
            _animator.SetBool("Revived", revived);
            _revenantSimpleAttack.isAttacking = false;
            _revenantStrongAttack.isAttacking = false;
            _effectSystem.AddEffect(new InvincibilityEffect(1.5f));
            _effectSystem.AddEffect(new StunEffect(1.5f));
        }
        
    }

    public void ReviveEnd()
    {
        _effectSystem.AddEffect(new RevenantRageEffect(-1));
        _effectSystem.AddEffect(new UnstoppableEffect(-1));
    }
}
