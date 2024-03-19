using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIsStunned : Node
{
    private EffectSystem _effectSystem;

    public CheckIsStunned(EffectSystem effectSystem)
    {
        _effectSystem = effectSystem;
    }

    public override NodeState Evaluate()
    {
        state = _effectSystem.CheckIfStunned() ? NodeState.FAILURE : NodeState.SUCCESS;
        return state;
    }
}
