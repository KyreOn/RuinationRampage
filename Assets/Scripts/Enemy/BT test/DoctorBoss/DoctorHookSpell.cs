using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DoctorHookSpell : MonoBehaviour
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
    public bool isHit;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    
    public bool StartAttack(GameObject target)
    {
        if (!canCast) return false;
        var distance = (target.transform.position - transform.position).magnitude;
        if (distance is < 4 or > 10) return false;
        _target = target;
        isHit = false;
        canCast = false;
        _animator.SetTrigger("HookCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        return true;
    }

    public void HookCast()
    {
        var rayDirection = _target.transform.position - spawnPoint.position;
        rayDirection.y = 0;
        if (!Physics.Raycast(spawnPoint.position, rayDirection, out var hit, float.PositiveInfinity, layerMask)) return;
        if (!hit.transform.CompareTag("Player")) return;
        transform.rotation = Quaternion.LookRotation(rayDirection);
        var hook = Instantiate(projectile, spawnPoint.position, transform.rotation);
        hook.GetComponent<DoctorHook>().Init(gameObject);
    }


    public void OnEnd()
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
