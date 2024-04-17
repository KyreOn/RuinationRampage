using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBlock : Spell
{
    private CharacterController _controller;
    private Animator            _animator;
    private MovementSystem      _movementSystem;
    private float               _blockTimer;
    private bool                _isBlocking;
    private bool                _isReset;
    
    [SerializeField] private float[] cooldown = new float[5];
    [SerializeField] private float[] cooldownReduction = new float[5];

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _movementSystem = GetComponent<MovementSystem>();
    }
    
    protected override void OnPrepare()
    {
        _isBlocking = true;
        _isReset = false;
        _blockTimer = 0;
        _animator.SetBool("Block",  true);
        _animator.SetBool("Slowed", true);
        _effectSystem.AddEffect(new SlowEffect(1, 2), false);
        _effectSystem.AddEffect(new ParryEffect(2 * (PlayerPrefs.GetString($"ChosenPerks1").Contains('1') ? 1.2f : 1), 1), false);
        Cast();
    }
    
    protected override void OnCast()
    {
        
    }

    protected override void OnUpdate()
    {
        if (!_isBlocking) return;
        _blockTimer += Time.deltaTime;
        if (_blockTimer >= 1)
        {
            _animator.SetBool("Block",  false);
            _animator.SetBool("Slowed", false);
            _isBlocking = false;
        }
    }

    public void CdReset()
    {
        _cooldownTimer = Mathf.Clamp(_cooldownTimer + cooldownReduction[level - 1], 0, baseCooldown);
    }
    
    protected override void OnUpgrade()
    {
        baseCooldown = cooldown[level - 1];
    }
    
    public override string GetDescription()
    {
        if (level == 0)
            return "Герой прикрывается щитом блокируя весь получаемый урон. При успешном блокировании перезарядка сокращается";
        var cdDiff     = cooldown[level] - cooldown[level - 1];
        var cdrDiff     = cooldownReduction[level] - cooldownReduction[level - 1];
        return $"КД: {cdDiff}с\nСнижение КД: +{cdrDiff}с";
    }
}
