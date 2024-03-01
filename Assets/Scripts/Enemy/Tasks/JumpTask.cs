using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class JumpTask : Node
{
    private GameObject _target;
    private GameObject _self;
    private JumpAttack _jumpAttack;
    private bool       _isInit;
    
    public JumpTask()
    {
    }
    
    private void Initialize()
    {
        try
        {
            _jumpAttack = ((GameObject)GetData("self")).GetComponent<JumpAttack>();
        }
        catch (NullReferenceException)
        {
            _isInit = false;
            return;
        }
        _isInit = true;
    }
    
    public override NodeState Evaluate()
    {
        if (!_isInit)
            Initialize();
        
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
