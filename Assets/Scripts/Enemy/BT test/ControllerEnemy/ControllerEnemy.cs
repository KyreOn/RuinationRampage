using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override bool OnCheckIsIdle()
    {
        return true;
    }
}
