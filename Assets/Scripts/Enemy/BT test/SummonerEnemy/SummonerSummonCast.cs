using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class SummonerSummonCast : Leaf<ITreeContext>
{
    private SummonerEnemy       _summoner;
    private SummonerSummonSpell _summonerSummonSpell;
    private bool                _started;

    protected override void OnSetup()
    {
        _summoner = Agent as SummonerEnemy;
        _summonerSummonSpell = _summoner.GetComponent<SummonerSummonSpell>();
    }

    protected override void OnEnter()
    {
        if (_summoner.CheckIsIdle())
        {
            var result = _summonerSummonSpell.StartAttack(_summoner.Player.gameObject);
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
        if (_started && !_summonerSummonSpell.isAttacking)
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
