using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class DoctorBlastCast : Leaf<ITreeContext>
{
    private DoctorBoss      _doctor;
    private DoctorBlastSpell _doctorBlastSpell;
    private bool            _started;

    protected override void OnSetup()
    {
        _doctor = Agent as DoctorBoss;
        _doctorBlastSpell = _doctor.GetComponent<DoctorBlastSpell>();
    }

    protected override void OnEnter()
    {
        if (_doctor.CheckIsIdle())
        {
            var result = _doctorBlastSpell.StartAttack(_doctor.Player.gameObject);
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
        if (_started && !_doctorBlastSpell.isAttacking)
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
