using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttack : MonoBehaviour
{
    [SerializeField] private float     jumpCooldown;
    [SerializeField] private LayerMask playerLayer;
    
    private float               _jumpCooldownTimer;
    private bool                _isPreparing;
    private bool                _canJump = true;
    private NavMeshAgent        _navMeshAgent;
    private EffectSystem        _effectSystem;
    private Animator            _animator;
    private GameObject          _target;
    private CharacterController _targetController;
    private Vector3             _jumpTarget;
    
    public bool       isJump;
    public Collider[] _playerCollider;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        _jumpCooldownTimer = jumpCooldown;
    }

    public bool StartJump(GameObject target)
    {
        if (!_canJump) return false;
        if (_navMeshAgent.path.corners.Length > 2) return false;
        _target = target;
        _targetController = target.GetComponent<CharacterController>();
        _playerCollider = new []{_target.GetComponent<Collider>()};
        _animator.SetTrigger("isJumping");
        _animator.speed = 0.3f;
        _canJump = false;
        _jumpCooldownTimer = 0;
        isJump = true;
        _isPreparing = true;
        _navMeshAgent.radius = 0.11f;
        return true;
    }
    
    void Jump()
    {
        _animator.speed = 1;
        _isPreparing = false;
        //_effectSystem.AddEffect(new StunImmuneEffect(1));
    }
    
    void StrongAttack()
    { 
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(1.5f, 2, 2f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            _target.GetComponent<DamageSystem>().ApplyDamage(10);
        }
    }

    void EndJump()
    {
        isJump = false;
    }

    public bool IsInAir()
    {
        return isJump && !_isPreparing;
    }

    private void Update()
    {
        if (_isPreparing)
        {
            if (_navMeshAgent.path.corners.Length <= 2)
            {
                var distance = _target.transform.position - transform.position;
                distance.Scale(new Vector3(0.8f, 0, 0.8f));
                var speed = _targetController.velocity;
                _jumpTarget = transform.position + distance;
                var rotation = Quaternion.LookRotation(_jumpTarget - transform.position);
                transform.rotation = rotation;
                _navMeshAgent.SetDestination(_jumpTarget);
            }
        }

        if (isJump)
        {
            _navMeshAgent.avoidancePriority = (int)(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 100);
        }

        if (IsInAir() && !_effectSystem.CheckForDisplacementEffect())
        {
            transform.position = Vector3.Lerp(transform.position, _jumpTarget, Time.deltaTime * 6);
        }
        
        if (_canJump) return;
        _jumpCooldownTimer += Time.deltaTime;
        if (_jumpCooldownTimer >= jumpCooldown)
        {
            _canJump = true;
        }

        
    }
}
