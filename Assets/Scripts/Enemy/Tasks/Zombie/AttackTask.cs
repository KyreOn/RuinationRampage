using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackTask : Node
{
    private GameObject   _target;
    private GameObject   _self;
    private ZombieAttack _zombieAttack;
    private bool         _isInit;
    
    public AttackTask()
    {
    }
    
    private void Initialize()
    {
        try
        {
            _zombieAttack = ((GameObject)GetData("self")).GetComponent<ZombieAttack>();
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
