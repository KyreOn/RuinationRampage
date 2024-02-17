using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class aimove : MonoBehaviour
{ 
    private                  NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject   player;
    [SerializeField] private float        attackRange;
    [SerializeField] private float        attackCooldown;
    [SerializeField] private float        jumpMinDistance;
    [SerializeField] private float        jumpMaxDistance;
    [SerializeField] private float        jumpCooldown;
    [SerializeField] private float        navCooldown;
    [SerializeField] private LayerMask    playerLayer;
    
    private Animator  _animator;
    private float     _attackCooldownTimer;
    private float     _jumpCooldownTimer;
    private float     _navCooldownTimer;
    private bool      _isAttacking;
    private bool      _isJump;
    public Collider[]      _playerCollider;
    private Transform _target;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerCollider = new []{player.GetComponent<Collider>()};
        _target = player.transform;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _jumpCooldownTimer = jumpCooldown;
        navMeshAgent.SetDestination(_target.position);
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        _attackCooldownTimer = Mathf.Clamp(_attackCooldownTimer + Time.deltaTime, 0, attackCooldown);
        _jumpCooldownTimer = Mathf.Clamp(_jumpCooldownTimer + Time.deltaTime, 0, jumpCooldown);
        _navCooldownTimer = Mathf.Clamp(_navCooldownTimer   + Time.deltaTime, 0, navCooldown);
        var distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= jumpMaxDistance && distance >= jumpMinDistance && _jumpCooldownTimer >= jumpCooldown && !_isAttacking)
        {
            _jumpCooldownTimer = 0;
            _navCooldownTimer = 0;
            _isJump = true;
        }

        if (_isJump)
        {
            StartJump();
            _isJump = false;
        }

        if (distance <= attackRange)
        {
            var direction = _target.position - transform.position;
            direction.Scale(new Vector3(1, 0, 1));
            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000);
        }
        if (distance <= attackRange && _attackCooldownTimer >= attackCooldown)
        {
            _isAttacking = true;
            _attackCooldownTimer = 0;
            StartAttack();
        }
        
        if (_navCooldownTimer >= navCooldown)
        {
            navMeshAgent.speed = 4;
            navMeshAgent.SetDestination(_target.position);
        }
    }

    void StartJump()
    {
        navMeshAgent.speed = 0;
        _animator.SetTrigger("isJumping");
        _animator.speed = 0.3f;
    }

    void Jump()
    {
        navMeshAgent.speed = 100;
        _animator.speed = 1;
        var distance = _target.position - transform.position;
        distance.Scale(new Vector3(1f, 0, 1f));
        var jumpTarget = transform.position + distance;
        navMeshAgent.SetDestination(jumpTarget);
    }

    void EndJump()
    {
        navMeshAgent.speed = 0;
    }

    void StartAttack()
    {
        _animator.SetTrigger("Attack");
    }

    void Attack()
    {
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(1.2f, 2, 1.2f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            player.GetComponent<HitEffect>().ApplyDamage();
        }
    }

    void StopAttack()
    {
        _isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + Vector3.forward + Vector3.up, new Vector3(1, 2, 1));
    }
}
