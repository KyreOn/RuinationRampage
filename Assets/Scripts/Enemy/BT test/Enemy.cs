using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : TreeAgent
{
    [SerializeField] protected float baseSpeed;
    [SerializeField] protected int   id;
    
    protected NavMeshAgent   navMeshAgent;
    protected EffectSystem   effectSystem;
    protected ParticleSystem xpParticleSys;
    protected ParticleSystem hpParticleSys;
    
    public PlayerTest Player         { get; set; }
    public bool       MovingToPlayer { get; set; }
    public bool       PlayerInSight  { get; set; }
    public bool       RotateOnMove   { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        effectSystem = GetComponent<EffectSystem>();
        xpParticleSys = FindObjectOfType<XPCollector>().GetComponent<ParticleSystem>();
        hpParticleSys = FindObjectOfType<HPCollector>().GetComponent<ParticleSystem>();
        Player = FindObjectOfType<PlayerTest>();
        xpParticleSys.trigger.SetCollider(0, Player.GetComponent<Collider>());
        hpParticleSys.trigger.SetCollider(0, Player.GetComponent<Collider>());
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
        return !effectSystem.CheckIfStunned() && OnCheckIsIdle();
    }

    public virtual bool OnCheckIsIdle()
    {
        return true;
    }

    public void OnDeath()
    {
        xpParticleSys.transform.position = transform.position + Vector3.up;
        hpParticleSys.transform.position = transform.position + Vector3.up;
        xpParticleSys.Emit(20);
        var choice = Random.value - 0.6f;
        var count  = choice > 0 ? Random.Range(1, 6) : 0;
        hpParticleSys.Emit((int)count);
        Destroy(gameObject);
        WaveManager.CheckForEnemies();
        StatsManager.AddKill(id);
    }
}
