using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BTree;
using UnityEngine;
using UnityEngine.AI;

public class AssassinEnemy : Enemy
{
    private AssassinEnemyDash _assassinEnemyDash;
    
    protected override void Awake()
    {
        base.Awake();
        _assassinEnemyDash = GetComponent<AssassinEnemyDash>();
    }

    public override bool OnCheckIsIdle()
    {
        return !(_assassinEnemyDash.isAttacking);
    }

    public void Reset()
    {
        Restart();
    }
    
    protected override void OnStun()
    {
        _assassinEnemyDash.isAttacking = false;
    }
}
