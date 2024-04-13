using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DoctorCageSpell : MonoBehaviour
{
    [SerializeField] private GameObject aoe;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  layerMask;

    private GameObject   _target;
    private EffectSystem _effectSystem;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private bool         _isDrawing;
    private float        _castCooldownTimer;
    private Collider[]   _player = new Collider[1];
    
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
        _animator.SetTrigger("CageCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        return true;
    }

    public void CageCast()
    {
        Instantiate(aoe, _target.transform.position - Vector3.up, transform.rotation);
    }

    public void CageCastEnd()
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
