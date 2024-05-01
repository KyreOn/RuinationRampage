using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantSimpleAttack : MonoBehaviour
{
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  playerLayer;
    [SerializeField] private GameObject slashEffect1;
    [SerializeField] private GameObject slashEffect2;
    [SerializeField] private GameObject rageSlashEffect1;
    [SerializeField] private GameObject rageSlashEffect2;
    [SerializeField] private GameObject lightingEffect;
    
    private GameObject    _target;
    private Animator      _animator;
    private EffectSystem  _effectSystem;
    private RevenantEnemy _revenantEnemy;
    private float         _attackCooldownTimer;
    private bool          _canAttack;
    private bool          _combo;
    
    public bool       isAttacking;
    public Collider[] _playerCollider;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        _revenantEnemy = GetComponent<RevenantEnemy>();
    }
    
    public void Slash()
    {
        var slash = Instantiate(_combo ? (_revenantEnemy.revived ? rageSlashEffect1 : slashEffect1) : (_revenantEnemy.revived ? rageSlashEffect2 : slashEffect2), transform.position + Vector3.up, transform.rotation);
        _combo = !_combo;
        Destroy(slash, 1);
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
        var random = Random.value;
        _animator.SetTrigger(_combo ? "SimpleAttack1" : "SimpleAttack2");
        _animator.speed = _revenantEnemy.revived ? 1.5f : 1;
        return true;
    }

    void Attack()
    { 
        if (_revenantEnemy.revived)
        {
            var lighting = Instantiate(lightingEffect, transform.position + transform.forward * 1.4f,
                Quaternion.identity);
            Destroy(lighting, 1);
        }
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(2f, 2, 2f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            if (_target.GetComponent<DamageSystem>().ApplyDamage(10, transform) && _revenantEnemy.revived)
                _target.GetComponent<EffectSystem>().AddEffect(new SlowEffect(1, 1.25f));
        }
    }

    void StopAttack()
    {
        isAttacking = false;
        _animator.speed = 1;
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
