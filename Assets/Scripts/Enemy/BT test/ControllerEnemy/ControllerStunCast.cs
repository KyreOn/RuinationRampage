using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class ControllerStunCast : Leaf<ITreeContext>
{
    private                  ControllerEnemy  _controller;
    private                  ControllerStunSpell _controllerStunSpell;
    private                  bool         _started;

    protected override void OnSetup()
    {
        _controller = Agent as ControllerEnemy;
        _controllerStunSpell = _controller.GetComponent<ControllerStunSpell>();
    }

    protected override void OnEnter()
    {
        if (_controller.CheckIsIdle())
        {
            var result = _controllerStunSpell.StartAttack(_controller.Player.gameObject);
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
        if (_controller.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        if (_started && !_controllerStunSpell.isAttacking)
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
