using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class GetInRangeTask : Node
{
    private GameObject   _target;
    private GameObject   _self;
    private NavMeshAgent _navAgent;
    private EffectSystem _effectSystem;
    private Animator     _animator;
    private float        _updateTime;
    private float        _updateCounter;
    private bool         _isToUpdate;
    private float        _baseSpeed;
    private float        _curSpeed;
    private bool         _isInit;
    
    public GetInRangeTask(float updateTime)
    {
        _updateTime = updateTime;
    }

    private void Initialize()
    {
        try
        {
            _baseSpeed = (float)GetData("speed");
            _self = (GameObject)GetData("self");
            _navAgent = _self.GetComponent<NavMeshAgent>();
            _effectSystem = _self.GetComponent<EffectSystem>();
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
            var direction   = _self.transform.position   - _target.transform.position;
            var targetPoint = _target.transform.position + direction.normalized * 8;
            _updateCounter = 0;
            _isToUpdate = true;
            
            _navAgent.SetDestination(targetPoint);
        }

        state = NodeState.SUCCESS;
        return state;
    }
    
    private void CalculateSpeed()
    {
        _curSpeed = _baseSpeed * _effectSystem.CalculateSpeedModifiers() * (_effectSystem.CheckIfDisabled() ? 0 : 1);
    }
}
