using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BTree;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : TreeAgent
{
    [SerializeField] private float baseSpeed;
    
    private NavMeshAgent navMeshAgent;
    private EffectSystem _effectSystem;
    private ZombieAttack _simpleAttack;
    private JumpAttack   _jumpAttack;
    
    public  PlayerTest   Player         { get; set; }
    public  bool         MovingToPlayer { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        _effectSystem = GetComponent<EffectSystem>();
        Player = FindObjectOfType<PlayerTest>();
        _simpleAttack = GetComponent<ZombieAttack>();
        _jumpAttack = GetComponent<JumpAttack>();
    }

    public Vector3 MoveTo(Vector3 position)
    {
        if (navMeshAgent.enabled == false)
            return Vector3.zero;
        Vector3 pos;
        if (NavMesh.SamplePosition(position, out var hit, 4f, NavMesh.AllAreas))
        {
            pos = hit.position;
        }
        else
        {
            pos = Vector3.zero;
        }

        navMeshAgent.SetDestination(pos);
        navMeshAgent.speed = baseSpeed * _effectSystem.CalculateSpeedModifiers();
        //Debug.DrawLine(transform.position, pos, Color.red, 2f);
        return pos;
    }

    public bool CheckIsIdle()
    {
        return !(_simpleAttack.isAttacking || _jumpAttack.isJump);
    }
}
