using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterProjectile : MonoBehaviour
{
    [SerializeField] private float      speed;
    [SerializeField] private float      maxHeight;
    [SerializeField] private float      explosionRadius;
    [SerializeField] private GameObject indicator;

    private GameObject _indicatorInstance;
    private Vector3    _target;
    private float      _timer;
    private Curve      _trajectory;
    private bool       _isInit;
    
    public void Initialize(Vector3 target)
    {
        _target = target;
        _target.y = 0.1f;
        var controlPoint = (transform.position + _target) / 2 + Vector3.up * maxHeight;
        _trajectory = new Curve(transform.position, _target, controlPoint);
        _indicatorInstance = Instantiate(indicator, _target, Quaternion.identity);
        _isInit = true;
    }
    
    void Update()
    {
        if (!_isInit) return;
        
        _timer += Time.deltaTime * speed;
        transform.position = _trajectory.Evaluate(_timer);
        var forward = _trajectory.Evaluate(_timer + 0.001f) - _trajectory.Evaluate(_timer);
        transform.forward = forward == Vector3.zero ? Vector3.down : forward;

        if (_timer >= 1)
        {
            Destroy(gameObject);
            Destroy(_indicatorInstance);
            var hitTargets = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var target in hitTargets)
            {
                if (target.CompareTag("Player"))
                    target.GetComponent<DamageSystem>().ApplyDamage();
            }
        }
    }
}
