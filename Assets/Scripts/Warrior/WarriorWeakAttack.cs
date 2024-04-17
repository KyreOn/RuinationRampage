using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWeakAttack : Spell
{
    [SerializeField] private LayerMask  _enemyLayer;
    [SerializeField] private GameObject model;
    [SerializeField] private float[]    damage = new float[5];
    
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
        _animator.SetBool("WeakAttack", true);
        _animator.SetBool("Slowed",     true);
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("WeakAttack", false);
    }

    public void StartWeak()
    {
        _animator.speed = 2;
        _effectSystem.AddEffect(new WeakAttackEffect(2), false);
    }
    
    public void WeakHit()
    {
        //_effectSystem.AddEffect(new DisplacementEffect(0.05f, model.transform.forward, 0.25f));
        _attackCombo = !_attackCombo;
        _animator.SetBool("WeakAttackCombo", _attackCombo);
        var collider = Physics.OverlapBox(transform.position + model.transform.forward * 2, Vector3.one * 2, model.transform.rotation,
            _enemyLayer);
        var stunMod = PlayerPrefs.GetString($"ChosenPerks1").Contains('0') ? 1.2f : 1;
        foreach (var col in collider)
        {
            var dmg = damage[level - 1] * _effectSystem.CalculateOutcomeDamage() *
                         (col.GetComponent<EffectSystem>().CheckIfStunned()
                ? stunMod
                : 1);
            if (col.GetComponent<DamageSystem>().ApplyDamage(dmg))
                _rSpell.StackDamage(dmg);
        }
    }

    public void StopAttack()
    {
        _animator.SetBool("Slowed", false);
        _effectSystem.RemoveEffectById(12);
        _effectSystem.RemoveEffectById(13);
        _animator.speed = 1;
    }
    
    public override string GetDescription()
    {
        var damageDiff = damage[level] - damage[level - 1];
        return $"Урон: +{damageDiff}";
    }
}
