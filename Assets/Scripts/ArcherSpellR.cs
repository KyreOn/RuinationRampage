using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellR : Spell
{
    [SerializeField] private GameObject   indicator;
    [SerializeField] private LayerMask    wallLayer;
    [SerializeField] private LayerMask    enemyLayer;
    [SerializeField] private LineRenderer laserBeam;
    [SerializeField] private Transform    laserSpawnPoint;

    [SerializeField] private float[] cooldown    = new float[3];
    [SerializeField] private float[] prepareTime = new float[3];
    [SerializeField] private float[] damage      = new float[3];
    [SerializeField] private int[]   charges     = new int[3];
    
    private CharacterController _controller;
    private EffectSystem        _effectSystem;
    private Animator            _animator;
    private GameObject          _indicator;
    private Vector3             _clampedPosition;
    private GameObject          _model;
    private bool                _isShoot;
    private float               _beamProgress;
    private bool                _isCharging;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _effectSystem = GetComponent<EffectSystem>();
        _animator = GetComponent<Animator>();
        isUlt = true;
    }

    protected override void OnPrepare()
    {
        _controller.enabled = false;
        _animator.SetBool("RSpell", true);
        _animator.SetBool("Charging", true);
    }

    public void SetModel(GameObject model)
    {
        _model = model;
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("Charging", false);
    }

    protected override void OnUpdate()
    {
        if (_isShoot)
        {
            _beamProgress += Time.deltaTime * 20;
            laserBeam.widthMultiplier = Mathf.Sin(_beamProgress);
            if (Mathf.Sin(_beamProgress) < 0)
            {
                _isShoot = false;
                _beamProgress = 0;
                laserBeam.enabled = false;
            }
        }
    }

    public void RSpellDraw()
    {
        _animator.speed = 0.4f * prepareTime[level-1];
    }
    
    public void RSpellShoot()
    {
        _animator.speed = 1f;
        laserBeam.enabled = true;
        _isShoot = true;
        var playerTransform = _model.transform;
        var ray             = new Ray(laserSpawnPoint.position, _model.transform.forward);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, wallLayer))
        {
            laserBeam.SetPosition(0, laserSpawnPoint.position);
            laserBeam.SetPosition(1, hit.point);
            var position     = playerTransform.position;
            var halfDistance = (hit.point - position) / 2;
            var enemies = Physics.OverlapBox(position + halfDistance,
                new Vector3(1, 2, halfDistance.magnitude), playerTransform.rotation, enemyLayer);
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<DamageSystem>().ApplyDamage(damage[level-1] * _effectSystem.CalculateOutcomeDamage());
            }
        }
    }

    public void RShootEnd()
    {
        _controller.enabled = true;
        _animator.SetBool("RSpell", false);
    }

    public override string GetDescription()
    {
        if (level == 0)
            return "После небольшой подготовки герой выпускает мощный луч, поражающий всех врагов на своем пути";
        var cdDiff      = cooldown[level] - cooldown[level - 1];
        var prepareDiff = Mathf.Round((prepareTime[level] - prepareTime[level - 1]) * 100);
        var damageDiff  = damage[level] - damage[level - 1];
        var chargeDiff  = (charges[level] - charges[level - 1]) == 0 ? "" : "Заряды: +1";
        return $"КД: {cdDiff}с\nВремя подготовки: -{prepareDiff}%\nУрон: +{damageDiff}\n{chargeDiff}";
    }

    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
        maxCharges = charges[level    - 1];
    }
}
