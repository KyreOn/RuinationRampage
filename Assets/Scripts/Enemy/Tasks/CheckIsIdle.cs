using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIsIdle : Node
{
    private JumpAttack   _jumpAttack;
    private ZombieAttack _zombieAttack;

    public CheckIsIdle(JumpAttack jumpAttack, ZombieAttack zombieAttack)
    {
        _jumpAttack = jumpAttack;
        _zombieAttack = zombieAttack;
    }

    public override NodeState Evaluate()
    {
        state = _jumpAttack.isJump || _zombieAttack.isAttacking ? NodeState.FAILURE : NodeState.SUCCESS;
        return state;
    }
}
