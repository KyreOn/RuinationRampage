using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : TreeAgent
{
    [SerializeField] protected float     baseSpeed;
    [SerializeField] protected int       id;
    [SerializeField] public    BossHPBar hpBar;

    private bool _isDisplaced;
    
    protected NavMeshAgent   navMeshAgent;
    protected EffectSystem   effectSystem;
    protected ParticleSystem xpParticleSys;
    protected ParticleSystem hpParticleSys;
    
    public PlayerTest Player         { get; set; }
    public bool       MovingToPlayer { get; set; }
    public bool       PlayerInSight  { get; set; }
    public bool       RotateOnMove   { get; set; }
    public bool       BlockRotation  { get; set; }
    
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
        RotateOnMove = false;
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (effectSystem.CheckIfStunned())
            OnStun();
        var pullEffect = effectSystem.CheckForPulled();
        var isPulled = pullEffect is not null;
        if (isPulled)
        {
            var position  = transform.position;
            var target    = ((PullingEffect)pullEffect).target;
            var direction = target - position;
            direction.y = 0;
            if (direction.magnitude < 0.5f)
                effectSystem.RemoveEffectById(16);
            direction.Normalize();
            transform.position = Vector3.MoveTowards(position, position + direction, Time.deltaTime * 50 * ((PullingEffect)pullEffect).speed);
        }
        _isDisplaced = effectSystem.CheckForDisplacementEffect();
        if (_isDisplaced)
        {
            var position  = transform.position;
            var direction = effectSystem.GetDisplacementDirection();
            if (Physics.Raycast(position, direction, 1))
                _isDisplaced = false;
            else
                transform.position = Vector3.MoveTowards(position, position + direction, Time.deltaTime * 50 * effectSystem.GetDisplacementSpeed());
        }
        
        if (!BlockRotation)
        {
            var dir = Player.Position - transform.position;
            dir.y = 0;
            var rot = Quaternion.LookRotation(dir);
            transform.forward = dir;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 10);
        }
        
        navMeshAgent.speed = baseSpeed * effectSystem.CalculateSpeedModifiers() * (CheckIsIdle() ? 1 : 0) * (effectSystem.CheckIfStunned() || effectSystem.CheckIfRooted() ? 0 : 1);
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

        navMeshAgent.updateRotation = false;
        
        
        
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
    
    protected virtual void OnStun()
    {
        
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
