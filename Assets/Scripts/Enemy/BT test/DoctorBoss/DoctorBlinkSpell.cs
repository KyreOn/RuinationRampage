using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DoctorBlinkSpell : MonoBehaviour
{
    [SerializeField] private GameObject aoe;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;
    [SerializeField] private LayerMask  layerMask;

    private GameObject   _target;
    private Vector3      _blinkTarget;
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
        _animator.SetTrigger("BlinkCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        var pos = Random.insideUnitCircle;
        pos *= 8;
        var position = new Vector3(pos.x, -1, pos.y);
        _blinkTarget = _target.transform.position + position;
        _effectSystem.AddEffect(new PullingEffect(_blinkTarget, 1));
        return true;
    }

    public void BlinkCast()
    {
        
    }
    
    private void Update()
    {
        if ((_blinkTarget - transform.position).magnitude < 1)
            isAttacking = false;
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
