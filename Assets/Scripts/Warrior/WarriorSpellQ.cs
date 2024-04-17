using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellQ : Spell
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int       enemyLayer;
    [SerializeField] private WarriorSpellQCollider  collider;
    
    [SerializeField] private float[] cooldown = new float[5];
    [SerializeField] private float[] damage   = new float[5];
    [SerializeField] private float[] distanceModifier  = new float[5];
    
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
        _effectSystem.AddEffect(new DisplacementEffect(0.15f * distanceModifier[level - 1], _direction, 1), false);
        _controller.excludeLayers |= 1 << enemyLayer;
        collider.Enable(damage[level - 1] * _effectSystem.CalculateOutcomeDamage(), 0.15f * distanceModifier[level - 1]);
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
    
    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
    }
    
    public override string GetDescription()
    {
        if (level == 0)
            return "Герой совершает рывок в выбранном направлении, сбивая всех врагов на своем пути и нанося им урон";
        var cdDiff     = cooldown[level] - cooldown[level - 1];
        var distDiff   = Mathf.Round((1 - distanceModifier[level] - (1 - distanceModifier[level - 1])) * 100);
        var damageDiff   = damage[level] - damage[level - 1];
        return $"КД: {cdDiff}с\nДальность: +{distDiff}%\nУрон: +{damageDiff}";
    }
}
