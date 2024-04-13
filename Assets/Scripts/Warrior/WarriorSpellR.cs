using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellR : Spell
{
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private int        enemyLayer;
    [SerializeField] private GameObject aoe;
    
    private CharacterController   _controller;
    private Animator              _animator;
    private WarriorMovementSystem _movementSystem;
    private Camera                _camera;
    private float                 _aoeTimer;
    public  bool                  _isCharging;
    private float                 _stackedDamage;
    private GameObject            _aoe;
    
    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<WarriorMovementSystem>();
    }
    
    protected override void OnPrepare()
    {
    }
    
    protected override void OnCast()
    {
        _animator.SetTrigger("SpellR");
    }

    public void Blast()
    {
        _aoeTimer = 0;
        _isCharging = true;
        _stackedDamage = 0;
        var cols = Physics.OverlapSphere(transform.position, 5, 1 << 9);
        _aoe = Instantiate(aoe, transform);
        _aoe.GetComponent<WarriorSpellRAoe>().Init(10, 1.5f);
        foreach (var enemy in cols)
        {
            var vectorDir = enemy.transform.position - transform.position;
            
            var direction = vectorDir.normalized;
            direction.y = 0;
            var distance  = 1 - (vectorDir.magnitude / 5);
            if (Physics.Raycast(new Ray(transform.position, direction), distance + 1, 1 << 10)) return;
            
            enemy.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.15f * distance, direction, 0.75f), false);
            enemy.GetComponent<EffectSystem>().AddEffect(new StunEffect(2f), false);
        }
        _effectSystem.AddEffect(new SlowEffect(10, 0.75f), false);
        _effectSystem.AddEffect(new OutcomeDamageEffect(10, 1.5f));
    }

    public void SecondBlast()
    {
        var cols = Physics.OverlapSphere(transform.position, 5, 1 << 9);
        foreach (var enemy in cols)
        {
            var vectorDir = enemy.transform.position - transform.position;
            
            var direction = vectorDir.normalized;
            direction.y = 0;
            var distance = 1                                                     - (vectorDir.magnitude / 5);
            if (Physics.Raycast(new Ray(transform.position, direction), distance + 1, 1 << 10)) return;
            
            enemy.GetComponent<DamageSystem>().ApplyDamage(10 + _stackedDamage);
        }
    }
    
    protected override void OnUpdate()
    {
        if (!_isCharging) return;
        _aoeTimer += Time.deltaTime;
        if (_aoeTimer >= 10)
        {
            SecondBlast();
            _isCharging = false;
        }
    }

    public void StackDamage(float damage)
    {
        _stackedDamage += damage;
    }
}
