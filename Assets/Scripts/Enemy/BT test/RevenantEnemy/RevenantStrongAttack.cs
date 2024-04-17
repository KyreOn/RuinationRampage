using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantStrongAttack : MonoBehaviour
{
    [SerializeField] private float     attackCooldown;
    [SerializeField] private LayerMask playerLayer;
    
    private GameObject    _target;
    private Animator      _animator;
    private EffectSystem  _effectSystem;
    private RevenantEnemy _revenantEnemy;
    private float         _attackCooldownTimer;
    private bool          _canAttack;
    
    public bool       isAttacking;
    public Collider[] _playerCollider;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        _revenantEnemy = GetComponent<RevenantEnemy>();
    }

    public bool StartAttack(GameObject target)
    {
        if (!_canAttack) return false;
        _target = target;
        _playerCollider = new []{_target.GetComponent<Collider>()};
        _canAttack = false;
        isAttacking = true;
        var direction = target.transform.position - transform.position;
        direction.Scale(new Vector3(1, 0, 1));
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
        _animator.SetTrigger("StrongAttack");
        _animator.speed = _revenantEnemy.revived ? 1.5f : 1;
        //_effectSystem.AddEffect(new SlowEffect(0.75f, 10000));
        return true;
    }

    void Attack()
    { 
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(2f, 2, 2f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            if (_target.GetComponent<DamageSystem>().ApplyDamage(10, transform))
                _target.GetComponent<EffectSystem>().AddEffect(new SlowEffect(3, _revenantEnemy.revived ? 1.75f : 1.5f));
        }
    }

    void StopAttack()
    {
        isAttacking = false;
    }
    
    private void Update()
    {
        if (_canAttack) return;
        _attackCooldownTimer += Time.deltaTime;
        if (!(_attackCooldownTimer >= attackCooldown)) return;
        _canAttack = true;
        _attackCooldownTimer = 0;
    }
}
