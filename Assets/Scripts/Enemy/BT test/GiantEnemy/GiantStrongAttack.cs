using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantStrongAttack : MonoBehaviour
{
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  playerLayer;
    [SerializeField] private Transform  hitPoint;
    [SerializeField] private GameObject hitIndicator;
    [SerializeField] private GameObject shockwaveEffect;
    
    
    private GameObject    _target;
    private Animator      _animator;
    private EffectSystem  _effectSystem;
    private RevenantEnemy _revenantEnemy;
    private float         _attackCooldownTimer;
    private bool          _canAttack = true;
    
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
        GetComponent<Enemy>().BlockRotation = true;
        //_effectSystem.AddEffect(new SlowEffect(1.5f, 10000));
        Instantiate(hitIndicator, transform.position + transform.forward * 4 + 0.3f * transform.right, Quaternion.identity);
        return true;
    }

    void StrongAttack()
    {
        var shockwave = Instantiate(shockwaveEffect, hitPoint.position, Quaternion.identity);
        Destroy(shockwave, 0.5f);
        var size      = Physics.OverlapSphereNonAlloc(hitPoint.position, 4f, _playerCollider, playerLayer);
        if (size >= 1)
        {
            if (_target.GetComponent<DamageSystem>().ApplyDamage(10, transform))
            {
                var direction = _playerCollider[0].transform.position - hitPoint.position;
                direction.y = 0;
                _target.GetComponent<EffectSystem>().AddEffect(new StunEffect(2));
                _target.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.75f, direction, 0.35f));
            }
                
        }
    }

    void StopAttack()
    {
        isAttacking = false;
        _animator.speed = 1;
        GetComponent<Enemy>().BlockRotation = false;
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
