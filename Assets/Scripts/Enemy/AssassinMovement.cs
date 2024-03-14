using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AssassinMovement : MonoBehaviour
{
    private Animator     _animator;
    private NavMeshAgent _navMeshAgent;
    private EffectSystem _effectSystem;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _effectSystem = GetComponent<EffectSystem>();
    }

    private void Update()
    {
        var forwardDot = Vector3.Dot(transform.forward, _navMeshAgent.velocity.normalized);
        var rightDot   = Vector3.Dot(transform.right, _navMeshAgent.velocity.normalized);
        _animator.SetFloat("Forward", forwardDot);
        _animator.SetFloat("Right", rightDot);
    }
}
