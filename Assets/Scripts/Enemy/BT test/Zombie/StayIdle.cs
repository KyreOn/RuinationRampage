using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class StayIdle : Leaf<ITreeContext>
{
    private Zombie _zombie;
    
    protected override void OnSetup()
    {
        _zombie = Agent as Zombie;
    }

    protected override void OnEnter()
    {
        var pos = _zombie.transform.position;
        var target = _zombie.MoveTo(pos);
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
