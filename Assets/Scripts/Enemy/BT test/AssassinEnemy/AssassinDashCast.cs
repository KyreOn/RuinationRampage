using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class AssassinDashCast : Leaf<ITreeContext>
{
    private AssassinEnemy     _assassin;
    private AssassinEnemyDash _assassinEnemyDash;
    private bool                _started;

    protected override void OnSetup()
    {
        _assassin = Agent as AssassinEnemy;
        _assassinEnemyDash = _assassin.GetComponent<AssassinEnemyDash>();
    }

    protected override void OnEnter()
    {
        if (_assassin.CheckIsIdle())
        {
            var result = _assassinEnemyDash.StartAttack(_assassin.Player.gameObject, _assassin.Player.Forward);
            if (!result)
                Response.Result = Result.Failure;
            else
                _started = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        if (_assassin.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        if (_started && !_assassinEnemyDash.isAttacking)
        {
            Response.Result = Result.Success;
        }
    }

    protected override void OnExit()
    {
        _started = false;
    }

    protected override void OnReset()
    {
        
    }

    protected override void OnFail()
    {
    }
}
