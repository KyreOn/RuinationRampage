using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class RevenantSimpleAttackTask : Leaf<ITreeContext>
{
    private RevenantEnemy      _revenantEnemy;
    private RevenantSimpleAttack _revenantSimpleAttack;
    private bool         _started;
    
    protected override void OnSetup()
    {
        _revenantEnemy = Agent as RevenantEnemy;
        _revenantSimpleAttack = _revenantEnemy.GetComponent<RevenantSimpleAttack>();
    }

    protected override void OnEnter()
    {
        if (_revenantEnemy.CheckIsIdle())
        {
            var result = _revenantSimpleAttack.StartAttack(_revenantEnemy.Player.gameObject);
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
        if (_revenantEnemy.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if (_started && !_revenantSimpleAttack.isAttacking)
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
