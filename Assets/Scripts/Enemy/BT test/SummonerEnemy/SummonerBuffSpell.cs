using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonerBuffSpell : MonoBehaviour
{
    [SerializeField] private GameObject buffCircle;
    [SerializeField] private GameObject buffAura;
    
    private GameObject[] _enemies;
    private EffectSystem _effectSystem;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private bool         _isDrawing;
    private GameObject   _buffCircle;
    private bool         _circleSpawned;
    
    public bool isAttacking;
    public bool isBuffing;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    
    public bool StartAttack()
    {
        _animator.SetTrigger("BuffCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        return true;
    }

    public void BuffCast()
    {
        isBuffing = true;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _effectSystem.AddEffect(new InvincibilityEffect(0.5f));
        foreach (var enemy in enemies)
        {
            if (enemy == gameObject) continue;
            enemy.GetComponent<EffectSystem>().AddEffect(new SlowEffect(0.5f, 0.5f), false);
            var aura = Instantiate(buffAura, enemy.transform);
            Destroy(aura, 0.5f);
        }
        if (_circleSpawned) return;
        _buffCircle = Instantiate(buffCircle, transform);
        _circleSpawned = true;
    }

    public void CastEnd()
    {
        isAttacking = false;
    }
    
    public void StopBuff()
    {
        _animator.speed = 1;
        isBuffing = false;
    }
    
    private void Update()
    {
        if (isBuffing && !_animator.GetBool("RunAway")) return;
        Destroy(_buffCircle);
        _circleSpawned = false;
    }
}
