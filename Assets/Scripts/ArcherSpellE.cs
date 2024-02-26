using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellE : MonoBehaviour
{
    [SerializeField] private GameObject projectile;

    private GameObject _indicator;
    private bool       _isCasting;
    private Vector3    _clampedPosition;
    private Transform    _spawnTransform;
    
    public void Prepare()
    {
        _isCasting = true;
    }

    public void Cast()
    {
        Instantiate(projectile, transform.position, _spawnTransform.rotation);
        _isCasting = false;
    }

    public void SetSpawnTransform(Transform spawnTransform)
    {
        _spawnTransform = spawnTransform;
    }
    
    private void Update()
    {
        
    }
}
