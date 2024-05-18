using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStrongAttack : Spell
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
        _animator.SetBool("StrongAttack", true);
        _animator.SetBool("Slowed", true);
        _effectSystem.AddEffect(new WeakAttackEffect(2), false);
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("StrongAttack", false);
    }

    public void StrongSlash()
    {
        var slash = Instantiate(_attackCombo ? slashEffect1 : slashEffect2, transform.position, model.transform.rotation);
        Destroy(slash, 1);
    }
    
    public void StrongHit()
    {
        //_effectSystem.AddEffect(new DisplacementEffect(0.05f, model.transform.forward, 0.25f));
        _attackCombo = !_attackCombo;
        _animator.SetBool("StrongAttackCombo", _attackCombo);
        var collider = Physics.OverlapBox(transform.position + model.transform.forward * 2.5f, Vector3.one * 2.5f, model.transform.rotation,
            _enemyLayer);
        var stunMod = PlayerPrefs.GetString($"ChosenPerks1").Contains('0') ? 1.2f : 1;
        foreach (var col in collider)
        {
            var dmg = damage[level - 1] * _effectSystem.CalculateOutcomeDamage() *
                      (col.GetComponent<EffectSystem>().CheckIfStunned()
                          ? stunMod
                          : 1);
            if (col.GetComponent<DamageSystem>().ApplyDamage(dmg, transform))
            {
                var hit = Instantiate(hitEffect, col.transform.position + Vector3.up - 0.5f * model.transform.forward,
                    Quaternion.Inverse(model.transform.rotation));
                Destroy(hit, 1);
                _rSpell.StackDamage(dmg);
            }
        }
    }
    
    public override string GetDescription()
    {
        if (level == 0)
            return "Герой выполняет тяжелую атаку, нанося урон всем задетым врагам в большом радиусе";
        var damageDiff = damage[level] - damage[level - 1];
        return $"Урон: +{damageDiff}";
    }
}
