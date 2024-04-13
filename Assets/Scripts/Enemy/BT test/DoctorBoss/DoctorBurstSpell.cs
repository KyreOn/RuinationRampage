using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DoctorBurstSpell : MonoBehaviour
{
    [SerializeField] private GameObject aoe;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  layerMask;

    private GameObject          _target;
    private EffectSystem        _effectSystem;
    private NavMeshAgent        _navMeshAgent;
    private Animator            _animator;
    private bool                _isDrawing;
    private float               _castCooldownTimer;
    private Collider[]          _player = new Collider[1];
    private CharacterController _cc;
    
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
        _cc = target.GetComponent<CharacterController>();
        canCast = false;
        _animator.SetTrigger("BurstCast");
        isAttacking = true;
        var pos = Random.insideUnitSphere;
        pos.y = 0;
        pos = pos.normalized * 4;
        GetComponent<Enemy>().MoveTo(transform.position + pos);
        return true;
    }

    public void BurstCast()
    {
        Instantiate(aoe, _target.transform.position - Vector3.up, transform.rotation);
    }

    public void BurstCastEnd()
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
                _castCooldownTimer = 0;
            }
        }
    }
}
