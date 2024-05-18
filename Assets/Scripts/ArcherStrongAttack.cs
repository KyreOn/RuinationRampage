using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStrongAttack : Spell
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject strongArrowObject;
    [SerializeField] private Transform  spawnPoint;
    
    [SerializeField] private float[] damage        = new float[5];
    [SerializeField] private float[] bleedDuration = new float[5];
    [SerializeField] private float[] bleedDamage   = new float[5];
    [SerializeField] private int[]   pierceCount   = new int[5];
    
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private bool                _isStrongAttacking;
    private float               _damageMultiplier;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
    }

    protected override void OnPrepare()
    {
        _isStrongAttacking = true;
        _animator.SetBool("isStrongAttack", _isStrongAttacking);
        _movementSystem.isAttacking = true;
    }
    
    protected override void OnCast()
    {
        _isStrongAttacking = false;
        _animator.SetBool("isStrongAttack", _isStrongAttacking);
        _movementSystem.isAttacking = false;
    }

    protected override void OnUpdate()
    {
        
    }
    
    public void StrongDraw()
    {
        _animator.SetBool("StrongCast", true);
        _movementSystem.isAttacking = true;
        _controller.enabled = false;
        _animator.speed = 1.2f * (PlayerPrefs.GetString($"ChosenPerks0").Contains('9') ? 0.8f : 1);
    }
    
    public void StrongShoot()
    {
        var arrow = Instantiate(strongArrowObject, spawnPoint.position, model.transform.rotation);
        arrow.GetComponent<StrongArrow>().Init(gameObject, damage[level-1] * _effectSystem.CalculateOutcomeDamage() * _damageMultiplier * (PlayerPrefs.GetString($"ChosenPerks0").Contains('9') ? 1.2f : 1), bleedDuration[level-1], bleedDamage[level-1], pierceCount[level-1]);
        if (PlayerPrefs.GetString($"ChosenPerks0").Contains('0'))
            _effectSystem.AddEffect(new SlowEffect(0.4f, 0.4f));
    }
    
    public void StrongShootEnd()
    {
        _animator.SetBool("StrongCast", false);
        _movementSystem.isAttacking = false;
        _controller.enabled = true;
        _animator.speed = 1;
    }

    public override string GetDescription()
    {
        if (level == 0)
            return "После небольшой задержки, герой выпускает стрелу, наносящая урон нескольким врагам на пути и накладывая на них кровотечение";
        var damageDiff   = damage[level]        - damage[level        - 1];
        var bleedDurDiff = bleedDuration[level] - bleedDuration[level - 1];
        var bleedDmgDiff = bleedDamage[level]   - bleedDamage[level   - 1];
        var pierceDiff   = (pierceCount[level] - pierceCount[level - 1]) == 0 ? "" : "Максимум целей: +1";
        return $"Урон: +{damageDiff}\nДлительность кровотечения: +{bleedDurDiff}с\nУрон за тик: +{bleedDmgDiff}\n{pierceDiff}";
    }
    
    public void OnHit()
    {
        if (PlayerPrefs.GetString($"ChosenPerks0").Contains('1'))
            _damageMultiplier = Mathf.Clamp(_damageMultiplier + 0.1f, 1, 1.5f);
    }
    
    public void OnMiss()
    {
        _damageMultiplier = 1;
    }
}
