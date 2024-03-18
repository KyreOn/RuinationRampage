using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class ArcherSimpleAttack : Leaf<ITreeContext>
{
    private ArcherEnemy      _archer;
    private ArcherAttack _archerAttack;
    private bool         _started;

    protected override void OnSetup()
    {
        _archer = Agent as ArcherEnemy;
        _archerAttack = _archer.GetComponent<ArcherAttack>();
    }

    protected override void OnEnter()
    {
        if (_archer.CheckIsIdle())
        {
            var result = _archerAttack.StartAttack(_archer.Player.gameObject);
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
        if (_archer.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if (_started && !_archerAttack.isAttacking)
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
