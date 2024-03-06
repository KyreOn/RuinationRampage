using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCaster : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override bool CheckIsIdle()
    {
        return true;
    }
}
