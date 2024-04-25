using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellE : Spell
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private float[]    cooldown   = new float[5];
    [SerializeField] private float[]    stunLength = new float[5];
    [SerializeField] private int[]      targets    = new int[5];
    
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private GameObject          _indicator;
    private bool                _isCasting;
    private Vector3             _clampedPosition;
    private Transform           _spawnTransform;
    private Camera              _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
    }
    
    protected override void OnPrepare()
    {
        _indicator = Instantiate(indicator);
        isBlocked = true;
    }

    protected override void OnCast()
    {
        Destroy(_indicator);
        _controller.enabled = false;
        _animator.SetBool("ESpell", true);
    }

    public void SetSpawnTransform(Transform spawnTransform)
    {
        _spawnTransform = spawnTransform;
    }

    protected override void OnUpdate()
    {
        if (!isPreparing) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
        {
            var position  = hit.point;
            var playerPos = transform.position;
            playerPos.Scale(new Vector3(1, 0, 1));
            var direction = (position - playerPos).normalized;
            //var distance  = Mathf.Clamp((position - playerPos).magnitude, 0, 6);
            _clampedPosition = playerPos + direction * 10;
            _indicator.transform.position = transform.position - transform.up;
            _indicator.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    public void ESpellDraw()
    {
        _animator.SetBool("ECast", true);
        _movementSystem.isAttacking = true;
        _animator.speed = 2.5f;
    }
    
    public void ESpellShoot()
    {
        _animator.speed = 1f;
        var proj = Instantiate(projectile, spawnPoint.position, model.transform.rotation);
        proj.GetComponent<ArcherSpellEProjectile>().Init(stunLength[level -1], targets[level -1], transform);
    }
    
    public void EShootEnd()
    {
        _animator.SetBool("ECast", false);
        _movementSystem.isAttacking = false;
        _controller.enabled = true;
        _animator.SetBool("ESpell", false);
        isBlocked = false;
    }

    public override string GetDescription()
    {
        if (level == 0)
            return "Герой выстреливает стрелой, которая связывает первого врага на пути и нескольких врагов за ним";
        var cdDiff     = cooldown[level]   - cooldown[level   - 1];
        var stunDiff   = stunLength[level] - stunLength[level - 1];
        var targetDiff = (targets[level] - targets[level - 1]) == 0 ? "" : "Максимум целей: +1";
        return $"КД: {cdDiff}с\nОглушение: +{stunDiff}с\n{targetDiff}";
    }

    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
    }
}
