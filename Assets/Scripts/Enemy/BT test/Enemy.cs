using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : TreeAgent
{
    [SerializeField] protected float baseSpeed;
    
    protected NavMeshAgent navMeshAgent;
    protected EffectSystem effectSystem;
    
    public PlayerTest Player         { get; set; }
    public bool       MovingToPlayer { get; set; }
    public bool       PlayerInSight  { get; set; }
    public bool       RotateOnMove   { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        effectSystem = GetComponent<EffectSystem>();
        Player = FindObjectOfType<PlayerTest>();
        RotateOnMove = true;
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

        navMeshAgent.updateRotation = RotateOnMove;
        navMeshAgent.SetDestination(pos);
        navMeshAgent.speed = baseSpeed * effectSystem.CalculateSpeedModifiers();
        Debug.DrawLine(transform.position, pos, Color.red, 2f);
        return pos;
    }

    public virtual bool CheckIsIdle()
    {
        return true;
    }
}
