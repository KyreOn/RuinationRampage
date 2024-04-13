using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellE : Spell
{
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private GameObject model;
    [SerializeField]
    
    private CharacterController   _controller;
    private Animator              _animator;
    private WarriorMovementSystem _movementSystem;
    private bool                  _isAiming;
    private Camera                _camera;
    private Vector3               _direction;
    private DamageSystem          _damageSystem;
    
    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<WarriorMovementSystem>();
        _damageSystem = GetComponent<DamageSystem>();
    }
    
    protected override void OnPrepare()
    {
        _isAiming = true;
    }
    
    protected override void OnCast()
    {
        _animator.SetTrigger("SpellE");
    }

    public void Bash()
    {
        var collider = Physics.OverlapBox(transform.position + model.transform.forward * 2, Vector3.one * 2, model.transform.rotation,
            1 << 9);
        foreach (var col in collider)
        {
            var vectorDir = col.transform.position - transform.position;
            var direction = vectorDir.normalized;
            direction.y = 0;
            var distance = 1                                                     - (vectorDir.magnitude / 5);
            if (Physics.Raycast(new Ray(transform.position, direction), distance + 1, 1 << 10)) return;
            if (col.GetComponent<DamageSystem>().ApplyDamage(10))
            {
                direction.y = 0;
                col.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.075f, direction, 0.5f));
                col.GetComponent<EffectSystem>().AddEffect(new StunEffect(0.175f));
            }
        }
        _effectSystem.AddEffect(new TemporaryHealthEffect(10 * collider.Length, -1));
        _damageSystem.ApplyHeal(0);
    }

    protected override void OnUpdate()
    {
        if (!_isAiming) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
        {
            var position  = hit.point;
            var playerPos = transform.position;
            playerPos.Scale(new Vector3(1, 0, 1));
            _direction = (position - playerPos).normalized;
        }
    }
}
