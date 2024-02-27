using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellR : Spell
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private LayerMask  wallLayer;
    [SerializeField] private LayerMask  enemyLayer;

    private GameObject _indicator;
    private bool       _isCasting;
    private Vector3    _clampedPosition;
    private GameObject _model;
    
    protected override void OnPrepare()
    {
        _isCasting = true;
    }

    public void SetModel(GameObject model)
    {
        _model = model;
    }
    
    protected override void OnCast()
    {
        var playerTransform = _model.transform;
        var ray             = new Ray(playerTransform.position, _model.transform.forward);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, wallLayer))
        {
            var position = playerTransform.position;
            //Debug.Log((hit.point - position).magnitude);
            var halfDistance = (hit.point - position) / 2;
            var enemies = Physics.OverlapBox(position + halfDistance,
                new Vector3(1, 2, halfDistance.magnitude), playerTransform.rotation, enemyLayer);
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<DamageSystem>().ApplyDamage();
            }
        }
    }

    protected override void OnUpdate()
    {
        
    }
}
