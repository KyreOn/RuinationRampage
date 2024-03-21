using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssassinEnemyDash : MonoBehaviour
{
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  playerLayer;

    private Collider[]   _playerCollider;
    private GameObject   _target;
    private Vector3      _targetForward;
    private EffectSystem _effectSystem;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private float        _castCooldownTimer;

    public bool canCast;
    public bool isAttacking;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    
    public bool StartAttack(GameObject target, Vector3 forward)
    {
        if (!canCast) return false;
        var dot = Vector3.Dot(forward, transform.forward);
        if (dot < 0.7 && !target.GetComponent<EffectSystem>().CheckIfRooted() && !target.GetComponent<EffectSystem>().CheckIfStunned()) return false;
        _target = target;
        _targetForward = forward;
        _playerCollider = new []{_target.GetComponent<Collider>()};
        canCast = false;
        //_animator.SetTrigger("DashCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        DashCast();
        return true;
    }

    public void DashCast()
    {
        transform.position = _target.transform.position - _targetForward * 1.5f;
        transform.forward = _targetForward;
        //_effectSystem.AddEffect(new SlowEffect(1.5f, 100000));
        Strike();
    }

    public void Strike()
    {
        _animator.SetTrigger("Attack");
    }
    
    public void Attack()
    { 
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(1.5f, 2, 2f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            _target.GetComponent<DamageSystem>().ApplyDamage(10);
            _target.GetComponent<EffectSystem>().AddEffect(new SlowEffect(1, 2));
            _target.GetComponent<EffectSystem>().AddEffect(new DOTEffect(2, 0.5f, 10));
        }
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
