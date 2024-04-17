using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSimpleAttack : MonoBehaviour
{
    [SerializeField] private float     attackCooldown;
    [SerializeField] private LayerMask playerLayer;
    
    private GameObject    _target;
    private Animator      _animator;
    private EffectSystem  _effectSystem;
    private float         _attackCooldownTimer;
    private bool          _canAttack = true;
    
    public bool       isAttacking;
    public Collider[] _playerCollider;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
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
        _animator.SetTrigger("SimpleAttack");
        return true;
    }

    void SimpleAttack()
    { 
        var size = Physics.OverlapBoxNonAlloc(transform.position + transform.forward + Vector3.up, new Vector3(3f, 3, 3f), _playerCollider, transform.rotation, playerLayer);
        if (size >= 1)
        {
            if (_target.GetComponent<DamageSystem>().ApplyDamage(10, transform))
            {
                _target.GetComponent<EffectSystem>().AddEffect(new StunEffect(2));
                _target.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.05f, transform.forward));
            }
                
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
