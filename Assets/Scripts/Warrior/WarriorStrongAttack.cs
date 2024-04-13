using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStrongAttack : Spell
{
    [SerializeField] private LayerMask  _enemyLayer;
    [SerializeField] private GameObject model;
    
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private bool                _attackCombo;
    private WarriorSpellR       _rSpell;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
        _rSpell = GetComponent<WarriorSpellR>();
    }
    
    protected override void OnPrepare()
    {
        _animator.SetBool("StrongAttack", true);
        _animator.SetBool("Slowed", true);
        _effectSystem.AddEffect(new WeakAttackEffect(2), false);
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("StrongAttack", false);
    }

    public void StrongHit()
    {
        //_effectSystem.AddEffect(new DisplacementEffect(0.05f, model.transform.forward, 0.25f));
        _attackCombo = !_attackCombo;
        _animator.SetBool("StrongAttackCombo", _attackCombo);
        var collider = Physics.OverlapBox(transform.position + model.transform.forward * 2, Vector3.one * 2, model.transform.rotation,
            _enemyLayer);
        foreach (var col in collider)
        {
            if (col.GetComponent<DamageSystem>().ApplyDamage(10))
                _rSpell.StackDamage(2);
        }
    }
}
