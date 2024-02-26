using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private float     attackCooldown;
    [SerializeField] private LayerMask playerLayer;
    
    private GameObject _target;
    private Animator   _animator;
    private float      _attackCooldownTimer;
    private bool       _canAttack;
    
    public bool       isAttacking;
    public Collider[] _playerCollider;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool StartAttack(GameObject target)
    {
        if (!_canAttack) return false;
        _target = target;
        _playerCollider = new []{_target.GetComponent<Collider>()};
        isAttacking = true;
        _animator.SetTrigger("Attack");
        return true;
    }

    void Attack()
    { 
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(1.5f, 2, 2f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            _target.GetComponent<DamageSystem>().ApplyDamage();
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
        if (_attackCooldownTimer >= attackCooldown)
        {
            _canAttack = true;
        }
    }
}
