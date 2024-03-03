using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class ArcherBT : Tree
{
    private GameObject   player;
    private NavMeshAgent _navMeshAgent;
    private EffectSystem _effectSystem;
    private Animator     _animator;
    private ArcherAttack _archerAttack;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        _archerAttack = GetComponent<ArcherAttack>();
        player = GameObject.FindWithTag("Player");
    }

    protected override Node SetupTree()
    {
        var rootNode = new Selector();
        rootNode.SetData("speed", 4f);
        rootNode.SetData("self",   gameObject);
        rootNode.SetData("player", player);

        var sequence = new Sequence(new List<Node>
        {
            new CheckInRange(10, 6),
            new ShootTask()
        });
        var closingSequence = new Sequence(new List<Node>
        {
            new CheckIsIdleArcher(_archerAttack),
            new GetInRangeTask(0.1f),
        });
        
        var moveSequence = new Sequence(new List<Node>
        {
            new CheckInRange(float.MaxValue, 0),
            new ChaseTask(0.1f)
        });
        
        
        //rootNode.Attach(jumpSequence);
        //rootNode.Attach(attackSequence);
        rootNode.Attach(sequence);
        rootNode.Attach(closingSequence);
        
        return rootNode;
    }
}
