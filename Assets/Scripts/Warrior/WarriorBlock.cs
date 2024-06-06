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
    
    [SerializeField] private float[]    cooldown          = new float[5];
    [SerializeField] private float[]    cooldownReduction = new float[5];
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject hitEffect;

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
        effectSystem.AddEffect(new SlowEffect(1, 2), false);
        effectSystem.AddEffect(new ParryEffect(2 * (PlayerPrefs.GetString($"ChosenPerks1").Contains('1') ? 1.2f : 1), 1.5f), false);
        Cast();
    }

    public void BlockStart()
    {
        shield.SetActive(true);
        _animator.SetBool("BlockLoop", true);
    }

    public void BlockLoopStart()
    {
        
    }

    public void StopAttack()
    {
        
    }
    

    protected override void OnUpdate()
    {
        if (!_isBlocking) return;
        _blockTimer += Time.deltaTime;
        if (_blockTimer >= 1)
        {
            shield.SetActive(false);
            _animator.SetBool("Block",  false);
            _animator.SetBool("BlockLoop", false);
            _animator.SetBool("Slowed", false);
            _isBlocking = false;
        }
    }

    public bool CheckForParry(Transform source)
    {
        var dot = Vector3.Dot(model.transform.forward, source.forward);
        if (dot <= -Mathf.Cos(Mathf.Deg2Rad * (PlayerPrefs.GetString($"ChosenPerks0").Contains('2') ? 90 : 60)))
        {
            CdReset();
            var hit = Instantiate(hitEffect, transform.position + model.transform.forward * 0.5f, model.transform.rotation);
            Destroy(hit, 1);
            return true;
        }

        return false;
    }
    
    public void CdReset()
    {
        cooldownTimer = Mathf.Clamp(cooldownTimer + cooldownReduction[level - 1], 0, baseCooldown);
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
