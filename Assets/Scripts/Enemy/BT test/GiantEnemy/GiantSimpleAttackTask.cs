using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class GiantSimpleAttackTask : Leaf<ITreeContext>
{
    private GiantEnemy      _giantEnemy;
    private GiantSimpleAttack _giantSimpleAttack;
    private bool         _started;
    
    protected override void OnSetup()
    {
        _giantEnemy = Agent as GiantEnemy;
        _giantSimpleAttack = _giantEnemy.GetComponent<GiantSimpleAttack>();
    }

    protected override void OnEnter()
    {
        if (_giantEnemy.CheckIsIdle())
        {
            var result = _giantSimpleAttack.StartAttack(_giantEnemy.Player.gameObject);
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
        if (_giantEnemy.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if (_started && !_giantSimpleAttack.isAttacking)
        {
            Debug.Log("end");
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
