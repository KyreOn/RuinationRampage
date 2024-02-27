using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellE : Spell
{
    [SerializeField] private GameObject projectile;

    private GameObject _indicator;
    private bool       _isCasting;
    private Vector3    _clampedPosition;
    private Transform    _spawnTransform;
    
    protected override void OnPrepare()
    {
        
    }

    protected override void OnCast()
    {
        Instantiate(projectile, transform.position, _spawnTransform.rotation);
    }

    public void SetSpawnTransform(Transform spawnTransform)
    {
        _spawnTransform = spawnTransform;
    }

    protected override void OnUpdate()
    {
        
    }
}
