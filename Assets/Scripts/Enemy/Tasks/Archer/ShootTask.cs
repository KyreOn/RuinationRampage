using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class ShootTask : Node
{
    private GameObject   _target;
    private GameObject   _self;
    private ArcherAttack _archerAttack;
    private bool         _isInit;
    private float        _timer;
    
    private void Initialize()
    {
        try
        {
            _archerAttack = ((GameObject)GetData("self")).GetComponent<ArcherAttack>();
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
        
        var result = _archerAttack.StartAttack((GameObject)GetData("player"));
        if (!result && !_archerAttack.isAttacking)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (_archerAttack.isAttacking)
        {
            state = NodeState.RUNNING;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
