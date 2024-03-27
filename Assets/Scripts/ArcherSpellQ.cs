using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellQ : Spell
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private float      minCastRange;
    [SerializeField] private float      maxCastRange;
    
    [SerializeField] private float[] cooldown   = new float[5];
    [SerializeField] private float[] slowPower  = new float[5];
    [SerializeField] private float[] tickDamage = new float[5];
    [SerializeField] private int[]   charges = new int[5];

    private CharacterController _controller;
    private EffectSystem        _effectSystem;
    private Animator            _animator;
    private GameObject          _indicator;
    private bool                _isCasting;
    private Vector3             _clampedPosition;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
    }
    
    protected override void OnPrepare()
    {
        _indicator = Instantiate(indicator);
    }
    
    protected override void OnCast()
    {
        _controller.enabled = false;
        _animator.SetBool("QSpell", true);
        Destroy(_indicator);
        _indicator = null;
    }

    protected override void OnUpdate()
    {
        if (!isPreparing) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
        {
            var position  = hit.point;
            var playerPos = transform.position;
            playerPos.Scale(new Vector3(1, 0, 1));
            var direction = (position - playerPos).normalized;
            var distance  = Mathf.Clamp((position - playerPos).magnitude, minCastRange, maxCastRange);
            _clampedPosition = playerPos + direction * distance;
            _indicator.transform.position = _clampedPosition;
        }
    }
    
    public void QSpellDraw()
    {
        _animator.speed = 2.5f;
    }
    
    public void QSpellShoot()
    {
        _animator.speed = 1f;
        var proj = Instantiate(projectile, _clampedPosition, Quaternion.identity);
        proj.GetComponent<ArcherSpellQProjectile>().Init(tickDamage[level-1] * _effectSystem.CalculateOutcomeDamage(), slowPower[level-1]);
    }
    
    public void QShootEnd()
    {
        _controller.enabled = true;
        _animator.SetBool("QSpell", false);
    }

    public override string GetDescription()
    {
        if (level == 0)
            return "Герой поражает выбранную область градом стрел, нанося урон всем зхадетым врагам и замедляя их";
        var cdDiff     = cooldown[level] - cooldown[level - 1];
        var slowDiff   = Mathf.Round((1 - slowPower[level] - (1 - slowPower[level - 1])) * 100);
        var tickDiff   = tickDamage[level] - tickDamage[level - 1];
        var chargeDiff = (charges[level] - charges[level - 1]) == 0 ? "" : "Заряды: +1";
        return $"КД: {cdDiff}с\nЗамедление: +{slowDiff}%\nУрон за тик: +{tickDiff}\n{chargeDiff}";
    }

    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
        maxCharges = charges[level    - 1];
    }
}
