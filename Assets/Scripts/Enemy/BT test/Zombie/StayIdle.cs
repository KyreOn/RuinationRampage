using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class StayIdle : Leaf<ITreeContext>
{
    private Enemy _enemy;
    
    protected override void OnSetup()
    {
        _enemy = Agent as Enemy;
    }

    protected override void OnEnter()
    {
        var pos = _enemy.transform.position;
        var target = _enemy.MoveTo(pos);
    }

    protected override void OnExecute()
    {
        Response.Result = Result.Success;
    }

    protected override void OnExit()
    {
        
    }

    protected override void OnReset()
    {
        
    }

    protected override void OnFail()
    {
        
    }
}
