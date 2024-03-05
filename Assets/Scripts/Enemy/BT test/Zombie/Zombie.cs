using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BTree;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy
{
    private ZombieAttack _simpleAttack;
    private JumpAttack   _jumpAttack;
    
    protected override void Awake()
    {
        base.Awake();
        _simpleAttack = GetComponent<ZombieAttack>();
        _jumpAttack = GetComponent<JumpAttack>();
    }

    public override bool CheckIsIdle()
    {
        return !(_simpleAttack.isAttacking || _jumpAttack.isJump);
    }
}
