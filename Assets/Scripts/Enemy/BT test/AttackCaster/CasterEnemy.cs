using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemy : Enemy
{
    private CasterSimpleAttack _casterSimpleAttack;
    private AttackCasterSpell  _attackCasterSpell;
    
    protected override void Awake()
    {
        base.Awake();
        _casterSimpleAttack = GetComponent<CasterSimpleAttack>();
        _attackCasterSpell = GetComponent<AttackCasterSpell>();
    }

    public override bool OnCheckIsIdle()
    {
        return !(_casterSimpleAttack.isAttacking || _attackCasterSpell.isAttacking);
    }
    
    protected override void OnStun()
    {
        _casterSimpleAttack.isAttacking = false;
        _attackCasterSpell.isAttacking = false;
    }
}
