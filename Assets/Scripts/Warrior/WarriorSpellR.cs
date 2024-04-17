using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellR : Spell
{
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private int        enemyLayer;
    [SerializeField] private GameObject aoe;
    
    [SerializeField] private float[] cooldown   = new float[5];
    [SerializeField] private float[] damage     = new float[5];
    [SerializeField] private float[] damageBuff = new float[5];
    [SerializeField] private float[] secondDamage     = new float[5];
    
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
        _aoe.GetComponent<WarriorSpellRAoe>().Init(0.5f * _effectSystem.CalculateOutcomeDamage() * (PlayerPrefs.GetString($"ChosenPerks1").Contains('8') ? 0.5f : 1), 1.5f);
        foreach (var enemy in cols)
        {
            var vectorDir = enemy.transform.position - transform.position;
            
            var direction = vectorDir.normalized;
            direction.y = 0;
            var distance  = 1 - (vectorDir.magnitude / 5);
            if (Physics.Raycast(new Ray(transform.position, direction), distance + 1, 1 << 10)) return;
            enemy.GetComponent<DamageSystem>().ApplyDamage(damage[level - 1] * _effectSystem.CalculateOutcomeDamage() * (PlayerPrefs.GetString($"ChosenPerks1").Contains('8') ? 0.5f : 1));
            enemy.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.15f * distance, direction, 0.75f), false);
            enemy.GetComponent<EffectSystem>().AddEffect(new StunEffect(2f * (PlayerPrefs.GetString($"ChosenPerks1").Contains('1') ? 1.2f : 1)), false);
        }
        _effectSystem.AddEffect(new SlowEffect(10, 0.75f), false);
        _effectSystem.AddEffect(new OutcomeDamageEffect(10, damageBuff[level - 1]));
        if (PlayerPrefs.GetString($"ChosenPerks1").Contains('7'))
            _effectSystem.AddEffect(new IncomeDamageEffect(10, 1 / damageBuff[level - 1]));
        if (PlayerPrefs.GetString($"ChosenPerks1").Contains('8'))
            _effectSystem.AddEffect(new StunImmuneEffect(10));
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
            
            enemy.GetComponent<DamageSystem>().ApplyDamage(_stackedDamage * secondDamage[level - 1] * _effectSystem.CalculateOutcomeDamage() * (PlayerPrefs.GetString($"ChosenPerks1").Contains('8') ? 0.5f : 1));
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
    
    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
    }
    
    public override string GetDescription()
    {
        if (level == 0)
            return "Герой выпускает волну, отталкивающую ближайших врагов и наносящую им урон. После герой получает ауру, дающую дополнительную скорость и усиление урона. После 10 секунд герой выпускает вторую волну наносящую урон, равный доле нанесенного урона за время умения";
        var cdDiff         = cooldown[level] - cooldown[level - 1];
        var damageDiff     = damage[level]   - damage[level   - 1];
        var buffDiff       = Mathf.Round((1 - damageBuff[level] - (1 - damageBuff[level - 1])) * 100);
        var damageModifier = Mathf.Round((1 - secondDamage[level] - (1 - secondDamage[level - 1])) * 100);
        
        return $"КД: {cdDiff}с\nУрон от первого взрыва: +{damageDiff}\nУсиление урона: +{buffDiff}%\nДоля от нанесенного урона: +{damageModifier}%";
    }
}
