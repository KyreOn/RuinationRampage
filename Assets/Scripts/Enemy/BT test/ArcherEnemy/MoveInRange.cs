using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class MoveInRange : Leaf<ITreeContext>
{
    [SerializeField] private float _minRange;
    [SerializeField] private float _maxRange;
    
    private Enemy   _enemy;
    private Vector3 _target;
    
    protected override void OnSetup()
    {
        _enemy = Agent as Enemy;
    }

    protected override void OnEnter()
    {
        if (_enemy.CheckIsIdle())
        {
            var pos = GetPointInRange();
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

        if ((_target - _enemy.Player.Position).magnitude > _maxRange)
        {
            var pos = GetPointInRange();
            _target = _enemy.MoveTo(pos);
            Debug.DrawLine(_enemy.Position, _target, Color.black, 1f);
        }

        var distance = (_enemy.transform.position - _enemy.Player.Position).magnitude;
        if (distance >= _minRange && distance <= _maxRange)
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

    private Vector3 GetPointInRange()
    {
        var range     = Random.Range(_minRange, _maxRange);
        var direction = (_enemy.transform.position - _enemy.Player.Position).normalized;
        return _enemy.Player.Position + direction * range;
    }
}
