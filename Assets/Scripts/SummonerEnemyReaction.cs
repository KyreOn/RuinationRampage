using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SummonerEnemyReaction : Reaction
{
    [SerializeField] private GameObject circleEffect;
    
    private Animator     _animator;
    private Vector3      _direction;
    private NavMeshAgent _navMeshAgent;
    
    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void OnReact(GameObject stimuli)
    {
        return;
        var rotationDir = stimuli.transform.position - transform.position;
        rotationDir.y = 0;
        var rotation = Quaternion.LookRotation(rotationDir);
        transform.rotation = rotation;
        _animator.SetTrigger("React");
        var hor       = Math.Sign(Random.value - 0.5f);
        var direction = Vector3.zero - transform.position;
        direction.y = 0;
        _direction = direction.normalized * 12;
    }

    public void CastReact()
    {
        _animator.speed = 1;
        var circle = Instantiate(circleEffect, transform.position, Quaternion.identity);
        Destroy(circle, 1);
        NavMesh.SamplePosition(transform.position + _direction, out var hit, 4f, NavMesh.AllAreas);
        transform.position = hit.position;
        circle = Instantiate(circleEffect, transform.position, Quaternion.identity);
        Destroy(circle, 1);
        _navMeshAgent.ResetPath();
    }
}
