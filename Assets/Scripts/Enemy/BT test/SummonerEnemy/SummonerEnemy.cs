using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SummonerEnemy : Enemy
{
    private Animator _animator;
    
    public bool runFromPlayer;
    
    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    protected override void OnUpdate()
    {
        _animator.SetBool("RunAway", runFromPlayer);
    }

    public override bool OnCheckIsIdle()
    {
        return true;
    }
}
