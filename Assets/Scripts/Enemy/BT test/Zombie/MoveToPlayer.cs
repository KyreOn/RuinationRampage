using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class MoveToPlayer : Leaf<ITreeContext>
{
    private Zombie  _zombie;
    private Vector3 _target;
    
    protected override void OnSetup()
    {
        _zombie = Agent as Zombie;
    }

    protected override void OnEnter()
    {
        var pos = GetPlayerPos();
        _target = _zombie.MoveTo(pos);
        _zombie.MovingToPlayer = true;
    }

    protected override void OnExecute()
    {
        if (_zombie.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if ((_target - _zombie.Player.Position).magnitude > 1.4f)
        {
            var pos = GetPlayerPos();
            _target = _zombie.MoveTo(pos);
            Debug.DrawLine(_zombie.Position, _target, Color.black, 1f);
        }

        if ((_zombie.transform.position - _zombie.Player.Position).sqrMagnitude < 3f)
        {
            Response.Result = Result.Success;
        }
    }

    protected override void OnExit()
    {
        _zombie.MovingToPlayer = false;
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
        return _zombie.Player.Position;
    }
}
