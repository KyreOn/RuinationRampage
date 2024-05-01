using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DoctorBlastSpell : MonoBehaviour
{
    [SerializeField] private GameObject aoe;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  layerMask;
    [SerializeField] private GameObject blastEffect;
    
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
        _animator.SetTrigger("BlastCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        return true;
    }

    public void BlastCast()
    {
        var blast = Instantiate(blastEffect, transform.position, Quaternion.identity);
        Destroy(blast, 1);
        if (Physics.OverlapSphereNonAlloc(transform.position, 2.5f, _player, 1 << 8) == 1)
            _player[0].gameObject.GetComponent<DamageSystem>().ApplyDamage(10);
        var area  = Instantiate(aoe,         transform.position, transform.rotation);
        area.GetComponent<DoctorBlastAoe>().Init(10, 1.2f);
    }

    public void BlastCastEnd()
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
