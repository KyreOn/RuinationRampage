using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class ZombieBT : Tree
{
    [SerializeField] private GameObject player;
    
    private NavMeshAgent _navMeshAgent;
    private EffectSystem _effectSystem;
    private Animator     _animator;
    private JumpAttack   _jumpAttack;
    private ZombieAttack _zombieAttack;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        _jumpAttack = GetComponent<JumpAttack>();
        _zombieAttack = GetComponent<ZombieAttack>();
        player = GameObject.FindWithTag("Player");
    }

    protected override Node SetupTree()
    {
        var rootNode = new Selector();
        rootNode.SetData("speed", 4f);
        rootNode.SetData("self",   gameObject);
        rootNode.SetData("player", player);
        
        var moveSequence = new Sequence(new List<Node>
        {
            new CheckIsIdle(_jumpAttack, _zombieAttack),
            new CheckInRange(float.MaxValue, 0),
            new ChaseTask(0.1f)
        });
        var jumpSequence = new Sequence(new List<Node>
        {
            new CheckIsStunned(_effectSystem),
            new CheckInRange(6, 4),
            new CheckIsIdle(_jumpAttack, _zombieAttack),
            new RotateToTargetTask(gameObject, player),
            new JumpTask()
        });
        var attackSequence = new Sequence(new List<Node>
        {
            new CheckIsStunned(_effectSystem),
            new CheckInRange(2),
            new CheckIsIdle(_jumpAttack, _zombieAttack),
            new RotateToTargetTask(gameObject, player),
            new AttackTask()
        });
        
        rootNode.Attach(jumpSequence);
        rootNode.Attach(attackSequence);
        rootNode.Attach(moveSequence);
        
        return rootNode;
    }
}
