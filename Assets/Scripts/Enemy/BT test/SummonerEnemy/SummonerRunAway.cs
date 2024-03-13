using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class SummonerRunAway : Leaf<ITreeContext>
{
    [SerializeField] private LayerMask wallLayer;
    
    private SummonerEnemy _summoner;
    private Vector3       _target;
    
    protected override void OnSetup()
    {
        _summoner = Agent as SummonerEnemy;
    }

    protected override void OnEnter()
    {
        if (_summoner.CheckIsIdle())
        {
            _summoner.RotateOnMove = true;
            var direction = GetDirection();
            _target = _summoner.MoveTo(direction);
            _summoner.runFromPlayer = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        if (_summoner.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if ((_target - _summoner.transform.position).magnitude < 0.8f)
        {
            var direction = GetDirection();
            _target = _summoner.MoveTo(direction);
            _summoner.runFromPlayer = true;
        }

        if ((_summoner.transform.position - _summoner.Player.Position).magnitude >= 6)
        {
            Response.Result = Result.Success;
        }
    }

    protected override void OnExit()
    {
        _summoner.runFromPlayer = false;
    }

    protected override void OnReset()
    {
        _target = default;
    }

    protected override void OnFail()
    {
    }

    private Vector3 GetDirection()
    {
        var direction = _summoner.transform.position - _summoner.Player.Position;
        direction.y = 0;
        if (Physics.Raycast(_summoner.transform.position, direction, 2, wallLayer))
        {
            direction = _summoner.transform.right;
            if (Physics.Raycast(_summoner.transform.position, direction, 2, wallLayer))
            {
                direction = _summoner.transform.right * -1;
            }
        }
        return _summoner.transform.position + direction.normalized * 2;
    }
}
