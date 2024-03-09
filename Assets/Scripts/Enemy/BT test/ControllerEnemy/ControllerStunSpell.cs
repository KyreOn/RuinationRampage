using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerStunSpell : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  layerMask;

    private GameObject   _target;
    private EffectSystem _effectSystem;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private bool         _isDrawing;
    private float        _castCooldownTimer;

    public bool canCast = true;
    public bool isAttacking;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    
    public bool StartAttack(GameObject target)
    {
        if (!canCast) return false;
        _target = target;
        canCast = false;
        _animator.SetTrigger("StunCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        return true;
    }

    public void StunCast()
    {
        var rayDirection = _target.transform.position - spawnPoint.position;
        rayDirection.y = 0;
        if (!Physics.Raycast(spawnPoint.position, rayDirection, out var hit, float.PositiveInfinity, layerMask)) return;
        if (!hit.transform.CompareTag("Player")) return;
        transform.rotation = Quaternion.LookRotation(rayDirection);
        Instantiate(projectile, spawnPoint.position, transform.rotation);
    }

    public void CastEnd()
    {
        isAttacking = false;
    }
    
    private void Update()
    {
        if (!canCast)
        {
            _castCooldownTimer += Time.deltaTime;
            if (_castCooldownTimer >= attackCooldown)
            {
                canCast = true;
                isAttacking = false;
                _castCooldownTimer = 0;
            }
        }
    }
}
