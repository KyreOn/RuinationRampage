using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class CasterSimpleAttackCast : Leaf<ITreeContext>
{
    private                  CasterEnemy  _caster;
    private                  CasterSimpleAttack _casterSimpleAttack;
    private                  bool         _started;

    protected override void OnSetup()
    {
        _caster = Agent as CasterEnemy;
        _casterSimpleAttack = _caster.GetComponent<CasterSimpleAttack>();
    }

    protected override void OnEnter()
    {
        if (_caster.CheckIsIdle())
        {
            var result = _casterSimpleAttack.StartAttack(_caster.Player.gameObject);
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
        if (_caster.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        if (_started && !_casterSimpleAttack.isAttacking)
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
