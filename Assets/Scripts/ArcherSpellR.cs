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

    private CharacterController _controller;
    private Animator            _animator;
    private GameObject          _indicator;
    private Vector3             _clampedPosition;
    private GameObject          _model;
    private bool                _isShoot;
    private float               _beamProgress;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    protected override void OnPrepare()
    {
    }

    public void SetModel(GameObject model)
    {
        _model = model;
    }
    
    protected override void OnCast()
    {
        _controller.enabled = false;
        _animator.SetBool("RSpell", true);
        
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
        _animator.speed = 0.4f;
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
                enemy.GetComponent<DamageSystem>().ApplyDamage(10);
            }
        }
    }

    public void RShootEnd()
    {
        _controller.enabled = true;
        _animator.SetBool("RSpell", false);
    }
}
