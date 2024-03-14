using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinTrap : MonoBehaviour
{
    [SerializeField] private float lifespan;
    [SerializeField] private float activationTime;
    
    private float _lifespanTimer;
    private float _activationTimer;
    private bool  _isActive;

    private void Update()
    {
        _lifespanTimer += Time.deltaTime;
        if (_lifespanTimer >= lifespan)
        {
            Destroy(gameObject);
        }
        if (_isActive) return;
        _activationTimer += Time.deltaTime;
        if (_activationTimer >= activationTime)
            _isActive = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !_isActive) return;
        other.GetComponent<EffectSystem>().AddEffect(new StunEffect(2));
        Destroy(gameObject);
    }
}
