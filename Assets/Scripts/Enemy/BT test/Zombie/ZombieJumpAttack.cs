using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class ZombieJumpAttack : Leaf<ITreeContext>
{
    private Zombie     _zombie;
    private JumpAttack _jumpAttack;
    private bool       _started;
        
    protected override void OnSetup()
    {
        _zombie = Agent as Zombie;
        _jumpAttack = _zombie.GetComponent<JumpAttack>();
    }

    protected override void OnEnter()
    {
        if (_zombie.CheckIsIdle())
        {
            _jumpAttack.StartJump(_zombie.Player.gameObject);
            _started = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        if (_zombie.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if (_started && !_jumpAttack.isJump)
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
