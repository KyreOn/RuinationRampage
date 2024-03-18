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

    protected override void OnUpdate()
    {
        base.OnUpdate();
        navMeshAgent.speed = baseSpeed * effectSystem.CalculateSpeedModifiers() * (CheckIsIdle() ? 1 : 0);
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
        
        if (!RotateOnMove)
        {
            var dir = Player.Position - transform.position;
            dir.y = 0;
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 10);
        }
        
        navMeshAgent.SetDestination(pos);
        Debug.DrawLine(transform.position, pos, Color.red, 2f);
        return pos;
    }

    public bool CheckIsIdle()
    {
        return !effectSystem.CheckIfDisabled() && OnCheckIsIdle();
    }

    public virtual bool OnCheckIsIdle()
    {
        return true;
    }
}
