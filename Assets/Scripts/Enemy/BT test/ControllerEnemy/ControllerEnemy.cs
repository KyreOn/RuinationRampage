using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEnemy : Enemy
{
    private ControllerStunSpell _controllerStunSpell;
    
    protected override void Awake()
    {
        base.Awake();
        _controllerStunSpell = GetComponent<ControllerStunSpell>();
    }

    public override bool OnCheckIsIdle()
    {
        return !_controllerStunSpell.isAttacking;
    }
    
    protected override void OnStun()
    {
        _controllerStunSpell.isAttacking = false;
    }
}
