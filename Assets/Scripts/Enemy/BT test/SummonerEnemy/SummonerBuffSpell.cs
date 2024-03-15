using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonerBuffSpell : MonoBehaviour
{
    private GameObject[] _enemies;
    private EffectSystem _effectSystem;
    private NavMeshAgent _navMeshAgent;
    private Animator     _animator;
    private bool         _isDrawing;
    
    public bool isAttacking;
    
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
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _effectSystem.AddEffect(new InvincibilityEffect(0.5f));
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EffectSystem>().AddEffect(new SlowEffect(0.5f, 0.5f), false);
        }
    }

    public void CastEnd()
    {
        isAttacking = false;
    }
    
    private void Update()
    {
       
    }
}
