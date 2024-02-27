using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellQ : Spell
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private float      minCastRange;
    [SerializeField] private float      maxCastRange;

    private GameObject _indicator;
    private bool       _isCasting;
    private Vector3    _clampedPosition;
    
    protected override void OnPrepare()
    {
        _indicator = Instantiate(indicator);
    }
    
    protected override void OnCast()
    {
        Destroy(_indicator);
        _indicator = null;
        Instantiate(projectile, _clampedPosition, Quaternion.identity);
    }

    protected override void OnUpdate()
    {
        if (!isPreparing) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
        {
            var position  = hit.point;
            var playerPos = transform.position;
            playerPos.Scale(new Vector3(1, 0, 1));
            var direction = (position - playerPos).normalized;
            var distance  = Mathf.Clamp((position - playerPos).magnitude, minCastRange, maxCastRange);
            _clampedPosition = playerPos + direction * distance;
            _indicator.transform.position = _clampedPosition;
        }
    }
}
