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
    private Animator     _animator;
    private JumpAttack   _jumpAttack;
    private ZombieAttack _zombieAttack;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _jumpAttack = GetComponent<JumpAttack>();
        _zombieAttack = GetComponent<ZombieAttack>();
    }

    protected override Node SetupTree()
    {
        var root = new Selector();
        root.SetData("self",   gameObject);
        root.SetData("player", player);
        var moveSequence = new Sequence(new List<Node>
        {
            new CheckIsIdle(_jumpAttack, _zombieAttack),
            new CheckInRange(float.MaxValue, 0),
            new ChaseTask(_navMeshAgent, 0)
        });
        var jumpSequence = new Sequence(new List<Node>
        {
            new CheckInRange(6, 4),
            new CheckIsIdle(_jumpAttack, _zombieAttack),
            new RotateToTargetTask(gameObject, player),
            new JumpTask(_jumpAttack, 3)
        });
        var attackSequence = new Sequence(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckInRange(2),
                new CheckIsIdle(_jumpAttack, _zombieAttack),
                new RotateToTargetTask(gameObject, player),
                new AttackTask(_zombieAttack)
            })
            
        });
        
        root.Attach(jumpSequence);
        root.Attach(attackSequence);
        root.Attach(moveSequence);
        
        return root;
    }
}
