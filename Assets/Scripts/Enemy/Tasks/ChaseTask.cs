using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class ChaseTask : Node
{
    private GameObject   _target;
    private NavMeshAgent _navAgent;
    private Animator     _animator;
    private float        _updateTime;
    private float        _updateCounter;
    private bool         _isToUpdate;

    public ChaseTask(NavMeshAgent navAgent, float updateTime)
    {
        _navAgent = navAgent;
        _updateTime = updateTime;
    }
    
    public override NodeState Evaluate()
    {
        if (_isToUpdate)
        {
            _updateCounter += Time.deltaTime;
            if (_updateCounter >= _updateTime)
                _isToUpdate = false;
        }
        else
        {
            _target = (GameObject)GetData("player");
            _updateCounter = 0;
            _isToUpdate = true;
            _navAgent.speed = 4;
            _navAgent.SetDestination(_target.transform.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
