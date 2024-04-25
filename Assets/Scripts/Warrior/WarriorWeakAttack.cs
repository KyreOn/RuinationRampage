using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWeakAttack : Spell
{
    [SerializeField] private LayerMask  _enemyLayer;
    [SerializeField] private GameObject model;
    [SerializeField] private float[]    damage = new float[5];
    [SerializeField] private GameObject slashEffect1;
    [SerializeField] private GameObject slashEffect2;
    [SerializeField] private GameObject hitEffect;
    
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

    public void WeakSlash()
    {
        var slash = Instantiate(_attackCombo ? slashEffect1 : slashEffect2, transform.position, model.transform.rotation);
        Destroy(slash, 1);
    }
    
    public void WeakHit()
    {
        //_effectSystem.AddEffect(new DisplacementEffect(0.05f, model.transform.forward, 0.25f));
        _attackCombo = !_attackCombo;
        _animator.SetBool("WeakAttackCombo", _attackCombo);
        var collider = Physics.OverlapBox(transform.position + model.transform.forward * 1.6f, Vector3.one * 1.6f, model.transform.rotation,
            _enemyLayer);
        var stunMod = PlayerPrefs.GetString($"ChosenPerks1").Contains('0') ? 1.2f : 1;
        foreach (var col in collider)
        {
            var dmg = damage[level - 1] * _effectSystem.CalculateOutcomeDamage() *
                         (col.GetComponent<EffectSystem>().CheckIfStunned()
                ? stunMod
                : 1);
            if (col.GetComponent<DamageSystem>().ApplyDamage(dmg))
            {
                var hit = Instantiate(hitEffect, col.transform.position + Vector3.up - 0.5f * model.transform.forward, Quaternion.Inverse(model.transform.rotation));
                Destroy(hit, 1);
                _rSpell.StackDamage(dmg);
            }
                
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
