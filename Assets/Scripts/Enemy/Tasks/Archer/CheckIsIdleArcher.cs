using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CheckIsIdleArcher : Node
{
    private ArcherAttack _archerAttack;

    public CheckIsIdleArcher(ArcherAttack archerAttack)
    {
        _archerAttack = archerAttack;
    }

    public override NodeState Evaluate()
    {
        state = _archerAttack.isAttacking ? NodeState.FAILURE : NodeState.SUCCESS;
        return state;
    }
}
