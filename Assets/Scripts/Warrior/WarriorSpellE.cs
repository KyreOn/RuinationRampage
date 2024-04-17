using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellE : Spell
{
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private GameObject model;
    
    [SerializeField] private float[] cooldown         = new float[5];
    [SerializeField] private float[] damage           = new float[5];
    [SerializeField] private float[] healModifier = new float[5];
    
    private CharacterController   _controller;
    private Animator              _animator;
    private WarriorMovementSystem _movementSystem;
    private bool                  _isAiming;
    private Camera                _camera;
    private Vector3               _direction;
    private DamageSystem          _damageSystem;
    private float                 _tempHpTimer;
    
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
        _tempHpTimer = 0;
        var collider = Physics.OverlapBox(transform.position + model.transform.forward * 2, Vector3.one * 2, model.transform.rotation,
            1 << 9);
        foreach (var col in collider)
        {
            var vectorDir = col.transform.position - transform.position;
            var direction = vectorDir.normalized;
            direction.y = 0;
            var distance = 1                                                     - (vectorDir.magnitude / 5);
            if (Physics.Raycast(new Ray(transform.position, direction), distance + 1, 1 << 10)) return;
            if (col.GetComponent<DamageSystem>().ApplyDamage(damage[level - 1] * _effectSystem.CalculateOutcomeDamage()))
            {
                direction.y = 0;
                col.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.075f, direction, 0.5f));
                col.GetComponent<EffectSystem>().AddEffect(new StunEffect(0.175f * (PlayerPrefs.GetString($"ChosenPerks1").Contains('1') ? 1.2f : 1)));
            }
        }
        _effectSystem.AddEffect(new TemporaryHealthEffect(damage[level - 1] * _effectSystem.CalculateOutcomeDamage() * healModifier[level - 1] * collider.Length, PlayerPrefs.GetString($"ChosenPerks1").Contains('6') ? 15 : 10));
        _damageSystem.ApplyHeal(0);
    }

    protected override void OnUpdate()
    {
        if (_tempHpTimer > 10) return;
        _tempHpTimer += Time.deltaTime;
        if (_tempHpTimer > 10)
            _damageSystem.ApplyHeal(0);
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
            return "Герой бьет щитом, отталкивая задетых врагов и оглушая их. После герой получает временный барьер прочностью равной доле урона, нанесенного этим умением";
        var cdDiff     = cooldown[level] - cooldown[level - 1];
        var damageDiff = damage[level]   - damage[level   - 1];
        var healDiff   = Mathf.Round((1 - healModifier[level] - (1 - healModifier[level - 1])) * 100);
        return $"КД: {cdDiff}с\nУрон: +{damageDiff}\nДоля урона в барьер: +{healDiff}%";
    }
}
