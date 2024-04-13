using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class DoctorHookCast : Leaf<ITreeContext>
{
    private DoctorBoss      _doctor;
    private DoctorHookSpell _doctorHookSpell;
    private bool            _started;

    protected override void OnSetup()
    {
        _doctor = Agent as DoctorBoss;
        _doctorHookSpell = _doctor.GetComponent<DoctorHookSpell>();
    }

    protected override void OnEnter()
    {
        if (_doctor.CheckIsIdle())
        {
            var result = _doctorHookSpell.StartAttack(_doctor.Player.gameObject);
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
        if (_started && !_doctorHookSpell.isAttacking)
        {
            Response.Result = _doctorHookSpell.isHit ?  Result.Success : Result.Failure;
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
