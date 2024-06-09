using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellE : Spell
{
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private GameObject model;
    
    [SerializeField] private float[]    cooldown     = new float[5];
    [SerializeField] private float[]    damage       = new float[5];
    [SerializeField] private float[]    healModifier = new float[5];
    [SerializeField] private GameObject burstEffect;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject indicator;
    
    [Header("Sounds")] [SerializeField] private AudioClip spellSfx;
    
    private CharacterController   _controller;
    private Animator              _animator;
    private WarriorMovementSystem _movementSystem;
    private GameObject            _indicator;
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
        _indicator = Instantiate(indicator);
        _isAiming = true;
    }
    
    protected override void OnCast()
    {
        AudioManager.PlaySFX(spellSfx);
        Destroy(_indicator);
        _animator.SetTrigger("SpellE");
    }

    public void StartBash()
    {
        var burst = Instantiate(burstEffect, transform.position + model.transform.forward, model.transform.rotation);
        Destroy(burst, 1);
    }
    
    public void Bash()
    {
        CameraShakeManager.ApplyNoise(2f, 0.1f);
        _isAiming = false;
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
            if (col.GetComponent<DamageSystem>().ApplyDamage(damage[level - 1] * effectSystem.CalculateOutcomeDamage(), transform))
            {
                var hit = Instantiate(hitEffect, col.transform.position + Vector3.up - 0.5f * model.transform.forward,
                    Quaternion.Inverse(model.transform.rotation));
                Destroy(hit, 1);
                direction.y = 0;
                col.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.075f, direction, 0.5f));
                col.GetComponent<EffectSystem>().AddEffect(new StunEffect(1f * (PlayerPrefs.GetString($"ChosenPerks1").Contains('1') ? 1.2f : 1)));
            }
        }
        effectSystem.AddEffect(new TemporaryHealthEffect(damage[level - 1] * effectSystem.CalculateOutcomeDamage() * healModifier[level - 1] * collider.Length, PlayerPrefs.GetString($"ChosenPerks1").Contains('6') ? 15 : 10));
        _damageSystem.ApplyHeal(0);
    }

    protected override void OnUpdate()
    {
        if (isPreparing)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
            {
                var position  = hit.point;
                var playerPos = transform.position;
                playerPos.Scale(new Vector3(1, 0, 1));
                _direction = (position - playerPos).normalized;
                _indicator.transform.position = transform.position - transform.up;
                _indicator.transform.rotation = Quaternion.LookRotation(_direction);
            }
        }
        if (_tempHpTimer > 10) return;
        _tempHpTimer += Time.deltaTime;
        if (_tempHpTimer > 10)
            _damageSystem.ApplyHeal(0);
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
