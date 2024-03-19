using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class StrafeTask : Node
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
    private bool         _isInit;
    
    public StrafeTask(float updateTime)
    {
        _updateTime = updateTime;
    }

    private void Initialize()
    {
        try
        {
            _baseSpeed = (float)GetData("speed");
            _navAgent = ((GameObject)GetData("self")).GetComponent<NavMeshAgent>();
            _effectSystem = ((GameObject)GetData("self")).GetComponent<EffectSystem>();
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
            _updateCounter = 0;
            _isToUpdate = true;
            
            _navAgent.SetDestination(_target.transform.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
    
    private void CalculateSpeed()
    {
        _curSpeed = _baseSpeed * _effectSystem.CalculateSpeedModifiers() * (_effectSystem.CheckIfStunned() ? 0 : 1);
    }
}
