using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemy : Enemy
{
    private ArcherAttack _archerAttack;
    private EffectSystem _effectSystem;
    
    protected override void Awake()
    {
        base.Awake();
        _archerAttack = GetComponent<ArcherAttack>();
        _effectSystem = GetComponent<EffectSystem>();
    }

    public override bool OnCheckIsIdle()
    {
        return !(_archerAttack.isAttacking);
    }
}
