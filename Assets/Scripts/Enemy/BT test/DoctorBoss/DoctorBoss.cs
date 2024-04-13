using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorBoss : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override bool OnCheckIsIdle()
    {
        return true;
    }
    
    protected override void OnStun()
    {
    }
}
