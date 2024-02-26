using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttack : MonoBehaviour
{
    [SerializeField] private float jumpCooldown;
    
    private float        _jumpCooldownTimer;
    private bool         _isPreparing;
    private bool         _canJump = true;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private GameObject   _target;

    public bool isJump;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _jumpCooldownTimer = jumpCooldown;
    }

    public bool StartJump(GameObject target)
    {
        if (!_canJump) return false;
        if (_navMeshAgent.path.corners.Length > 2) return false;
        _target = target;
        _navMeshAgent.speed = 0;
        _animator.SetTrigger("isJumping");
        _animator.speed = 0.3f;
        _canJump = false;
        _jumpCooldownTimer = 0;
        isJump = true;
        _isPreparing = true;
        return true;
    }
    
    void Jump()
    {
        _navMeshAgent.speed = 100;
        _navMeshAgent.angularSpeed = 0;
        _animator.speed = 1;
        _isPreparing = false;
    }

    void EndJump()
    {
        _navMeshAgent.speed = 0;
        _navMeshAgent.angularSpeed = 10000;
        isJump = false;
    }

    private void Update()
    {
        if (_isPreparing)
        {
            if (_navMeshAgent.path.corners.Length <= 2)
            {
                var distance = _target.transform.position - transform.position;
                distance.Scale(new Vector3(0.8f, 0, 0.8f));
                var jumpTarget = transform.position + distance;
                _navMeshAgent.SetDestination(jumpTarget);
            }
        }
        if (_canJump) return;
        _jumpCooldownTimer += Time.deltaTime;
        if (_jumpCooldownTimer >= jumpCooldown)
        {
            _canJump = true;
        }
    }
}
