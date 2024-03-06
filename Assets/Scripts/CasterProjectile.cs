using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHeight;
    [SerializeField] private float explosionRadius;
    
    private Vector3 _target;
    private float   _timer;
    private Curve   _trajectory;
    private bool    _isInit;
    
    public void Initialize(Vector3 target)
    {
        _target = target;
        var controlPoint = (transform.position + _target) / 2 + Vector3.up * maxHeight;
        _trajectory = new Curve(transform.position, _target, controlPoint);
        _isInit = true;
    }
    
    void Update()
    {
        if (!_isInit) return;
        
        _timer += Time.deltaTime * speed;
        transform.position = _trajectory.Evaluate(_timer);
        transform.forward = _trajectory.Evaluate(_timer + 0.001f) - _trajectory.Evaluate(_timer);

        if (_timer >= 1)
        {
            Destroy(gameObject);
            var hitTargets = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var target in hitTargets)
            {
                if (target.CompareTag("Player"))
                    target.GetComponent<DamageSystem>().ApplyDamage();
            }
        }
    }
}
