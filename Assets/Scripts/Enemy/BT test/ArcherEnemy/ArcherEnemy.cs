using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemy : Enemy
{
    private ArcherAttack _archerAttack;
    
    protected override void Awake()
    {
        base.Awake();
        _archerAttack = GetComponent<ArcherAttack>();
    }

    public override bool CheckIsIdle()
    {
        return !(_archerAttack.isAttacking);
    }
}
