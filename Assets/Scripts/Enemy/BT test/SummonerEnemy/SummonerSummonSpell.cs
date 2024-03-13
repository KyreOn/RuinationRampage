using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SummonerSummonSpell : MonoBehaviour
{
    [SerializeField] private List<GameObject> summonedCreatures;
    [SerializeField] private float            attackCooldown;
    [SerializeField] private LayerMask        layerMask;
    [SerializeField] private int              maxSummoned;

    private GameObject       _target;
    private EffectSystem     _effectSystem;
    private NavMeshAgent     _navMeshAgent;
    private Animator         _animator;
    private bool             _isDrawing;
    private float            _castCooldownTimer;
    private List<GameObject> _summonedCreatures;

    public bool canCast = true;
    public bool isAttacking;
    
    private void Awake()
    {
        _effectSystem = GetComponent<EffectSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _summonedCreatures = new List<GameObject>();
    }
    
    public bool StartAttack(GameObject target)
    {
        if (!canCast) return false;
        if (_summonedCreatures.Count > 0)
        {
            foreach (var creature in _summonedCreatures)
            {
                if (!creature.Equals(null))
                    return false;
            }
        }
        
        _target = target;
        canCast = false;
        _animator.SetTrigger("SummonCast");
        isAttacking = true;
        _navMeshAgent.ResetPath();
        return true;
    }

    public void SummonCast()
    {
        _summonedCreatures.Clear();
        foreach (var summoned in from creature in summonedCreatures
                 let randomPoint = Random.insideUnitCircle.normalized * 2
                 let spawnPoint = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y)
                 select Instantiate(creature, spawnPoint, transform.rotation))
            _summonedCreatures.Add(summoned);
    }

    public void CastEnd()
    {
        isAttacking = false;
    }
    
    private void Update()
    {
        if (!canCast)
        {
            _castCooldownTimer += Time.deltaTime;
            if (_castCooldownTimer >= attackCooldown)
            {
                canCast = true;
                isAttacking = false;
                _castCooldownTimer = 0;
            }
        }
    }
}
