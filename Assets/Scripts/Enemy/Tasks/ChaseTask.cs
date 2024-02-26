using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class ChaseTask : Node
{
    private GameObject   _target;
    private NavMeshAgent _navAgent;
    private EffectSystem _effectSystem;
    private Animator     _animator;
    private float        _updateTime;
    private float        _updateCounter;
    private bool         _isToUpdate;
    private float        _baseSpeed;
    private float        _curSpeed;
    
    public ChaseTask(NavMeshAgent navAgent, EffectSystem effectSystem, float updateTime, float baseSpeed)
    {
        _navAgent = navAgent;
        _effectSystem = effectSystem;
        _updateTime = updateTime;
        _baseSpeed = baseSpeed;
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
            CalculateSpeed();
            _navAgent.speed = _curSpeed;
            _target = (GameObject)GetData("player");
            _updateCounter = 0;
            _isToUpdate = true;
            
            _navAgent.SetDestination(_target.transform.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
    
    private void CalculateSpeed()
    {
        _curSpeed = _baseSpeed * _effectSystem.CalculateSpeedModifiers();
    }
}
