using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class DoctorWaitTask : Leaf <ITreeContext >
{
    [SerializeField] private float time;
    
    private DoctorBoss       _doctor;
    private DoctorBlinkSpell _doctorBlinkSpell;
    private bool             _started;
    private float            _timer;
    
    protected override void OnSetup()
    {
        _doctor = Agent as DoctorBoss;
    }

    protected override void OnEnter()
    {
        _started = true;
    }

    protected override void OnExecute()
    {
        _timer += Time.deltaTime;
        if (_timer >= time)
            Response.Result = Result.Success;
    }

    protected override void OnExit()
    {
        _started = false;
        _timer = 0;
    }

    protected override void OnReset()
    {
        
    }

    protected override void OnFail()
    {
    }
}
