using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDodge : Spell
{
    [SerializeField] private float[]    cooldown   = new float[5];
    [SerializeField] private float[]    speedBoost = new float[5];
    [SerializeField] private GameObject trail;
    
    private CharacterController  _controller;
    private Animator             _animator;
    private DamageSystem         _damageSystem;
    private ArcherMovementSystem _movementSystem;
    private bool                 _spawned;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _damageSystem = GetComponent<DamageSystem>();
        _movementSystem = GetComponent<ArcherMovementSystem>();
    }

    protected override void OnPrepare()
    {
        _animator.SetTrigger("Dodge");
    }
    
    protected override void OnCast()
    {
    }

    protected override void OnUpdate()
    {
        
    }
    
    public void DodgeStart()
    {
        _movementSystem.OnDodgeStart();
        Cast();
        if (_spawned) return;
        var trailObj = Instantiate(trail, Vector3.zero, Quaternion.identity);
        trailObj.GetComponent<ArcherDodgeTrail>().Init(gameObject, PlayerPrefs.GetString($"ChosenPerks0").Contains('2') ? 1.7f : 1.2f);
        Destroy(trailObj, 2);
        _spawned = true;
    }
    
    public void DodgeEnd()
    {
        _movementSystem.OnDodgeEnd(speedBoost[level-1]);
        _spawned = false;
    }

    public override string GetDescription()
    {
        if (level == 0)
            return "Герой совершает рывок в сторону движения. " +
                   "Во время рывка герой неуязвим. "            +
                   "В конце рывка герой получает небольшой бонус к скорости";
        var cdDiff    = cooldown[level] - cooldown[level - 1];
        var speedDiff = Mathf.Round((1 - speedBoost[level] - (1 - speedBoost[level - 1])) * 100);
        return $"КД: {cdDiff}с\nУскорение: +{speedDiff}%";
    }

    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
    }
}
