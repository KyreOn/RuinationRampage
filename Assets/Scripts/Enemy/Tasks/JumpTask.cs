using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class JumpTask : Node
{
    private GameObject   _target;
    private GameObject   _self;
    private JumpAttack   _jumpAttack;
    
    public JumpTask(JumpAttack jumpAttack, float jumpTime)
    {
        _jumpAttack = jumpAttack;
    }
    
    public override NodeState Evaluate()
    {
        var result = _jumpAttack.StartJump((GameObject)GetData("player"));
        if (!result)
        {
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
