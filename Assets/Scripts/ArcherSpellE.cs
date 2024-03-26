using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellE : Spell
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float[] cooldown   = new float[5];
    [SerializeField] private float[] stunLength = new float[5];
    [SerializeField] private int[]   targets    = new int[5];
    
    private GameObject _indicator;
    private bool       _isCasting;
    private Vector3    _clampedPosition;
    private Transform    _spawnTransform;
    
    protected override void OnPrepare()
    {
        
    }

    protected override void OnCast()
    {
        baseCooldown = cooldown[level - 1];
        var proj = Instantiate(projectile, transform.position, _spawnTransform.rotation);
        proj.GetComponent<ArcherSpellEProjectile>().Init(stunLength[level-1], targets[level-1]);
    }

    public void SetSpawnTransform(Transform spawnTransform)
    {
        _spawnTransform = spawnTransform;
    }

    protected override void OnUpdate()
    {
        
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
}
