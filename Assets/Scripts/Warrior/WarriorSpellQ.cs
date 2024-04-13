using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellQ : Spell
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int       enemyLayer;
    [SerializeField] private WarriorSpellQCollider  collider;
    
    private CharacterController   _controller;
    private Animator              _animator;
    private WarriorMovementSystem _movementSystem;
    private bool                  _isAiming;
    private Camera                _camera;
    private Vector3               _direction;
    
    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<WarriorMovementSystem>();
    }
    
    protected override void OnPrepare()
    {
        _isAiming = true;
    }
    
    protected override void OnCast()
    {
        _animator.SetTrigger("SpellQ");
    }

    public void Charge()
    {
        _isAiming = false;
        _effectSystem.AddEffect(new DisplacementEffect(0.15f, _direction, 1), false);
        _controller.excludeLayers |= 1 << enemyLayer;
        collider.Enable();
    }

    public void ChargeEnd()
    {
        _controller.excludeLayers &= ~(1 << enemyLayer);
        collider.Disable();
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
