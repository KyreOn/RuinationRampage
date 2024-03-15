using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class MoveToPlayer : Leaf<ITreeContext>
{
    private Enemy  _enemy;
    private Vector3 _target;
    
    protected override void OnSetup()
    {
        _enemy = Agent as Enemy;
    }

    protected override void OnEnter()
    {
        if (_enemy.CheckIsIdle())
        {
            _enemy.RotateOnMove = true;
            var pos = GetPlayerPos();
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

        if ((_target - _enemy.Player.Position).magnitude > 0.25f)
        {
            var pos = GetPlayerPos();
            _target = _enemy.MoveTo(pos);
            Debug.DrawLine(_enemy.Position, _target, Color.black, 1f);
        }

        if ((_enemy.transform.position - _enemy.Player.Position).magnitude <= 2)
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

    private Vector3 GetPlayerPos()
    {
        return _enemy.Player.Position;
    }
}
