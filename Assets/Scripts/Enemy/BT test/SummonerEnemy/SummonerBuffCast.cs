using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class SummonerBuffCast : Leaf<ITreeContext>
{
    private SummonerEnemy     _summoner;
    private SummonerBuffSpell _summonerBuffSpell;
    private bool              _started;

    
    protected override void OnSetup()
    {
        _summoner = Agent as SummonerEnemy;
        _summonerBuffSpell = _summoner.GetComponent<SummonerBuffSpell>();
    }

    protected override void OnEnter()
    {
        if (_summoner.CheckIsIdle())
        {
            var result = _summonerBuffSpell.StartAttack();
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
        if (_summoner.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        Response.Result = Result.Success;
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
