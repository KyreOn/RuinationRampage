using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private Animator     _animator;
    private NavMeshAgent _navMeshAgent;
    private EffectSystem _effectSystem;
    private JumpAttack   _jumpAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _effectSystem = GetComponent<EffectSystem>();
        _jumpAttack = GetComponent<JumpAttack>();
    }

    private void Update()
    {
        var isStunned = _effectSystem.CheckIfDisabled() && !_jumpAttack.IsInAir();
        _animator.SetBool("Stunned", isStunned);
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }
}
