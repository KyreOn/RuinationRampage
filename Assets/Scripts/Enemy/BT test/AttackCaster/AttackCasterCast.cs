using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class AttackCasterCast : Leaf<ITreeContext>
{
    private AttackCaster _attackCaster;
    private AttackCasterSpell _attackCasterSpell;
    private bool         _started;
    
    protected override void OnSetup()
    {
        _attackCaster = Agent as AttackCaster;
        _attackCasterSpell = _attackCaster.GetComponent<AttackCasterSpell>();
    }

    protected override void OnEnter()
    {
        if (_attackCaster.CheckIsIdle())
        {
            _attackCasterSpell.StartAttack(_attackCaster.Player.gameObject);
            _started = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        if (_attackCaster.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        
        if (_started)
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
