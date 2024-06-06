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
    [SerializeField] private GameObject   flashEffect;
    [SerializeField] private GameObject   enemyHitEffect;
    [SerializeField] private GameObject   hitEffect;
    [SerializeField] private float        length;
    [SerializeField] private float        tickLength;
    [SerializeField] private LayerMask    groundLayer;
    
    [SerializeField] private float[] cooldown    = new float[3];
    [SerializeField] private float[] prepareTime = new float[3];
    [SerializeField] private float[] damage      = new float[3];
    
    [Header("Sounds")] [SerializeField] private AudioClip spellSfx;
    
    private Camera              _camera;
    private CharacterController _controller;
    private Animator            _animator;
    private ArcherMovementSystem      _movementSystem;
    private EffectSystem        _effectSystem;
    private GameObject          _indicator;
    private Vector3             _clampedPosition;
    private GameObject          _model;
    private bool                _isShoot;
    private float               _beamProgress;
    private bool                _isCharging;
    private float               _lengthTimer;
    private float               _tickTimer;
    
    
    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<ArcherMovementSystem>();
        _effectSystem = GetComponent<EffectSystem>();
        isUlt = true;
        _animator.SetInteger("RSpellLevel", level);
    }

    protected override void OnPrepare()
    {
        _indicator = Instantiate(indicator);
        isBlocked = true;
        _isCharging = true;
    }

    public void SetModel(GameObject model)
    {
        _model = model;
    }
    
    protected override void OnCast()
    {
        _animator.SetBool("RSpell",   true);
        _animator.speed = 1f;
    }

    protected override void OnUpdate()
    {
        if (_isCharging)
        {
            if (!_movementSystem.isDodge) _controller.enabled = false;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
            {
                var position  = hit.point;
                var playerPos = transform.position;
                playerPos.Scale(new Vector3(1, 0, 1));
                var direction = (position - playerPos).normalized;
                //var distance  = Mathf.Clamp((position - playerPos).magnitude, 0, 6);
                _clampedPosition = playerPos                       + direction * 10;
                _indicator.transform.position = transform.position - transform.up;
                _indicator.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        if (_isShoot)
        {
            if (!_movementSystem.isDodge) _controller.enabled = false;
            var playerTransform = _model.transform;
            var ray             = new Ray(laserSpawnPoint.position, _model.transform.forward);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, wallLayer))
            {
                laserBeam.SetPosition(0, laserSpawnPoint.position);
                laserBeam.SetPosition(1, hit.point);
                var flash = Instantiate(flashEffect, laserSpawnPoint.position, Quaternion.identity);
                Destroy(flash, 0.1f);
                var hitEff = Instantiate(hitEffect,   hit.point,                Quaternion.identity);
                Destroy(hitEff, 0.5f);
            }

            _tickTimer += Time.deltaTime;
            if (_tickTimer > tickLength)
            {
                _tickTimer = 0;
                CameraShakeManager.ApplyNoise(2f, 0.1f);
                var position     = playerTransform.position;
                var halfDistance = (hit.point - position) / 2;
                var enemies = Physics.OverlapBox(position + halfDistance,
                    new Vector3(1, 2, halfDistance.magnitude), playerTransform.rotation, enemyLayer);
                foreach (var enemy in enemies)
                {
                    var enemyHit = Instantiate(enemyHitEffect, enemy.transform.position + Vector3.up, Quaternion.identity);
                    Destroy(enemyHit, 0.3f);
                    enemy.GetComponent<DamageSystem>().ApplyDamage(damage[level-1] * _effectSystem.CalculateOutcomeDamage(), transform);
                }
            }

            _lengthTimer += Time.deltaTime;
            if (_lengthTimer > length)
            {
                _isShoot = false;
                laserBeam.enabled = false;
                return;
            }
            _beamProgress = (_lengthTimer / length) * Mathf.PI;
            laserBeam.widthMultiplier = Mathf.Sin(_beamProgress);
            if (Mathf.Sin(_beamProgress) < 0)
            {
                _beamProgress = 0;
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
        AudioManager.PlaySFX(spellSfx);
        Destroy(_indicator);
        _isCharging = false;
        _animator.SetFloat("RSpeed", 0.7f / length);
        _animator.speed = 1;
        laserBeam.enabled = true;
        _isShoot = true;
        _lengthTimer = 0;
        var playerTransform = _model.transform;
        var ray             = new Ray(laserSpawnPoint.position, _model.transform.forward);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, wallLayer))
        {
            laserBeam.SetPosition(0, laserSpawnPoint.position);
            laserBeam.SetPosition(1, hit.point);
            var hitEff = Instantiate(hitEffect, hit.point, Quaternion.identity);
            Destroy(hitEff, 0.5f);
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
    }
}
