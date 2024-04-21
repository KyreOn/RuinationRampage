using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellR : Spell
{
    [SerializeField] private GameObject     indicator;
    [SerializeField] private LayerMask      wallLayer;
    [SerializeField] private LayerMask      enemyLayer;
    [SerializeField] private LineRenderer   laserBeam;
    [SerializeField] private Transform      laserSpawnPoint;
    [SerializeField] private ParticleSystem effect;
    
    [SerializeField] private float[] cooldown    = new float[3];
    [SerializeField] private float[] prepareTime = new float[3];
    [SerializeField] private float[] damage      = new float[3];
    
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private EffectSystem        _effectSystem;
    private GameObject          _indicator;
    private Vector3             _clampedPosition;
    private GameObject          _model;
    private bool                _isShoot;
    private float               _beamProgress;
    private bool                _isCharging;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
        _effectSystem = GetComponent<EffectSystem>();
        isUlt = true;
        _animator.SetInteger("RSpellLevel", level);
    }

    protected override void OnPrepare()
    {
        _animator.SetBool("RSpell", true);
        _animator.SetBool("Charging", true);
        isBlocked = true;
    }

    public void SetModel(GameObject model)
    {
        _model = model;
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("Charging", false);
        _animator.speed = 1f;
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
        _movementSystem.isAttacking = true;
        _controller.enabled = false;
        _animator.SetFloat("RSpeed", 2 * prepareTime[level-1] * (PlayerPrefs.GetString($"ChosenPerks0").Contains('7') ? 1.5f : 1));
    }

    public void RSpellHold()
    {
        _movementSystem.isAttacking = true;
        _controller.enabled = false;
    }
    
    public void RSpellShoot()
    {
        _animator.speed = 1;
        laserBeam.enabled = true;
        _isShoot = true;
        var playerTransform = _model.transform;
        var ray             = new Ray(laserSpawnPoint.position, _model.transform.forward);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, wallLayer))
        {
            laserBeam.SetPosition(0, laserSpawnPoint.position);
            laserBeam.SetPosition(1, hit.point);
            effect.transform.position = hit.point;
            effect.Emit(1);
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
        _movementSystem.isAttacking = false;
        _controller.enabled = true;
        _animator.SetBool("RSpell", false);
        isBlocked = false;
    }

    public override string GetDescription()
    {
        if (level == 0)
            return "После небольшой подготовки герой выпускает мощный луч, поражающий всех врагов на своем пути";
        var cdDiff      = cooldown[level] - cooldown[level - 1];
        var prepareDiff = Mathf.Round((prepareTime[level] - prepareTime[level - 1]) * 100);
        var damageDiff  = damage[level] - damage[level - 1];
        var chargeDiff  = "Заряды: +1";
        return $"КД: {cdDiff}с\nВремя подготовки: -{prepareDiff}%\nУрон: +{damageDiff}\n{chargeDiff}";
    }

    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
        _animator.SetInteger("RSpellLevel", level);
    }
}
