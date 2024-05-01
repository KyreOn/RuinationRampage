using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AssassinReaction : Reaction
{
    [SerializeField] private float      strafeLength;
    [SerializeField] private LayerMask  wall;
    [SerializeField] private GameObject smokeEffect;
    
    private Animator          _animator;
    private Vector3           _direction;
    private NavMeshAgent      _navMeshAgent;
    private AssassinEnemyDash _assassinEnemyDash;
    private AssassinEnemy     _assassinEnemy;
    
    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _assassinEnemyDash = GetComponent<AssassinEnemyDash>();
        _assassinEnemy = GetComponent<AssassinEnemy>();
    }

    protected override void OnReact(GameObject stimuli)
    {
        if (_assassinEnemyDash.isAttacking) return;
        //_animator.SetTrigger("React");
        //_animator.speed = 4;
        if (effectSystem.CheckIfStunned()) return;
        var stimuliDir = stimuli.transform.position - transform.position;
        var hor       = Math.Sign(Vector3.SignedAngle(transform.forward, stimuliDir, Vector3.up)) * -1;
        var direction = transform.right * (hor * strafeLength);
        direction.y = 0;
        _direction = direction;
        NavMesh.SamplePosition(transform.position + _direction, out var hit, strafeLength, NavMesh.AllAreas);
        transform.position = hit.position;
        var smoke = Instantiate(smokeEffect, transform.position, Quaternion.identity);
        Destroy(smoke, 1);
        effectSystem.AddEffect(new SlowEffect(0.25f, 10000));
        effectSystem.AddEffect(new InvincibilityEffect(0.5f));
        _assassinEnemy.Reset();
    }

    public void CastReact()
    {
        _animator.speed = 1;
        
    }
}
