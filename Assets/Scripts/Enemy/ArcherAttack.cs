using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class ArcherAttack : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  layerMask;

    private GameObject   _target;
    private EffectSystem _effectSystem;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private bool         _isDrawing;
    private float        _attackCooldownTimer;

    public bool canAttack = true;
    public bool isAttacking;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public bool StartAttack(GameObject target)
    {
        if (!canAttack) return false;
        
        _effectSystem.AddEffect(new SlowEffect(1, 100000));
        _target = target;
        isAttacking = true;
        canAttack = false;
        _animator.SetBool("isAttacking", true);
        _isDrawing = true;
        _navMeshAgent.ResetPath();
        _navMeshAgent.updateRotation = false;
        return true;
    }

    public void Draw()
    {
        
    }
    
    public void Shoot()
    {
        _isDrawing = false;
        Instantiate(arrowObject, spawnPoint.position, transform.rotation);
        _animator.SetBool("isAttacking", false);
    }
    
    public void ShootEnd()
    {
        isAttacking = false;
        _navMeshAgent.updateRotation = true;
    }

    private void Update()
    {
        if (!canAttack)
        {
            _attackCooldownTimer += Time.deltaTime;
            if (_attackCooldownTimer >= attackCooldown)
            {
                canAttack = true;
                _attackCooldownTimer = 0;
            }
        }
        
        
        if (!_isDrawing) return;
        var rayDirection = _target.transform.position - spawnPoint.position;
        rayDirection.y = 0;
        if (!Physics.Raycast(spawnPoint.position, rayDirection, out var hit, float.PositiveInfinity, layerMask)) return;
        if (!hit.transform.CompareTag("Player")) return;
        var direction = _target.transform.position - spawnPoint.position;
        direction.Scale(new Vector3(1, 0, 1));
        transform.rotation = Quaternion.LookRotation(direction);

    }
}
