using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AttackCasterSpell : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;

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
        isAttacking = true;
        _animator.SetTrigger("StrongAttack");
        _navMeshAgent.ResetPath();
        return true;
    }

    public void CastStart()
    {
        _animator.speed = 0.5f;
    }
    
    public void StrongCast(float accuracy)
    {
        var proj = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
        var offset = Random.insideUnitCircle * accuracy;
        proj.GetComponent<CasterProjectile>().Initialize(_target.transform.position + new Vector3(offset.x, 0, offset.y));
    }
    
    public void CastEnd()
    {
        _animator.speed = 1;
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
