using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class DoctorBurstCast : Leaf<ITreeContext>
{
    private DoctorBoss      _doctor;
    private DoctorBurstSpell _doctorBurstSpell;
    private bool            _started;

    protected override void OnSetup()
    {
        _doctor = Agent as DoctorBoss;
        _doctorBurstSpell = _doctor.GetComponent<DoctorBurstSpell>();
    }

    protected override void OnEnter()
    {
        if (_doctor.CheckIsIdle())
        {
            var result = _doctorBurstSpell.StartAttack(_doctor.Player.gameObject);
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
        if (_doctor.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }
        if (_started && !_doctorBurstSpell.isAttacking)
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
