using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class AssassinTrapCast : Leaf<ITreeContext>
{
    private AssassinEnemy     _assassin;
    private AssassinTrapSpell _assassinTrapSpell;
    private bool                _started;

    protected override void OnSetup()
    {
        _assassin = Agent as AssassinEnemy;
        _assassinTrapSpell = _assassin.GetComponent<AssassinTrapSpell>();
    }

    protected override void OnEnter()
    {
        if (_assassin.CheckIsIdle())
        {
            var result = _assassinTrapSpell.StartAttack(_assassin.Player.gameObject);
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
        if (_started && !_assassinTrapSpell.isAttacking)
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
