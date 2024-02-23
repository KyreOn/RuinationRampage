using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackTask : Node
{
    private GameObject _target;
    private GameObject _self;
    private ZombieAttack _zombieAttack;
    
    public AttackTask(ZombieAttack zombieAttack)
    {
        _zombieAttack = zombieAttack;
    }
    
    public override NodeState Evaluate()
    {
        var result = _zombieAttack.StartAttack((GameObject)GetData("player"));
        if (!result)
        {
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
