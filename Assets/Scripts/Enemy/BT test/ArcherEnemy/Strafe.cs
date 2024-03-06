using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Strafe : Leaf<ITreeContext>
{
    [SerializeField] private int   strafeLength;
    
    private NavMeshAgent _navMeshAgent;
    private int          _strafeCounter;
    private Enemy        _enemy;
    private Vector3      _target;
    private int          _horDirection;
    
    protected override void OnSetup()
    {
        _enemy = Agent as Enemy;
        _navMeshAgent = _enemy.GetComponent<NavMeshAgent>();
    }

    protected override void OnEnter()
    {
        _horDirection = Math.Sign(Random.value - 0.5f);
        _strafeCounter = strafeLength;
        if (_enemy.CheckIsIdle())
        {
            _enemy.RotateOnMove = false;
            var pos = GetStrafePosition();
            _target = _enemy.MoveTo(pos);
            _enemy.MovingToPlayer = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        if (_enemy.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        
        if (_strafeCounter > 0 && _navMeshAgent.remainingDistance <= 0.5f)
        {
            var pos = GetStrafePosition();
            _strafeCounter--;
            _target = _enemy.MoveTo(pos);
        }
        
        if (_strafeCounter <= 0)
        {
            Response.Result = Result.Success;
        }
    }

    protected override void OnExit()
    {
        _enemy.MovingToPlayer = false;
    }

    protected override void OnReset()
    {
        _target = default;
    }

    protected override void OnFail()
    {
        
    }

    private Vector3 GetStrafePosition()
    {
        var direction     = Random.insideUnitCircle;
        var distance      = _enemy.Player.Position - _enemy.transform.position;
        var horDirection  = Vector3.Cross(distance, Vector3.up) * _horDirection;
        return _enemy.transform.position + horDirection.normalized;
    }
}
