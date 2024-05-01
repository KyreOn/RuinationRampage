using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinTrap : MonoBehaviour
{
    [SerializeField] private float lifespan;
    [SerializeField] private float activationTime;

    private Animator _animator;
    private Collider _collider;
    private float    _lifespanTimer;
    private float    _activationTimer;
    private bool     _isActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        _lifespanTimer += Time.deltaTime;
        if (_lifespanTimer >= lifespan)
        {
            _animator.SetTrigger("Trigger");
            Destroy(transform.parent.gameObject, 0.5f);
        }
        if (_isActive) return;
        _activationTimer += Time.deltaTime;
        if (_activationTimer >= activationTime)
        {
            _collider.enabled = true;
            _isActive = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !_isActive) return;
        _animator.SetTrigger("Trigger");
        other.GetComponent<EffectSystem>().AddEffect(new StunEffect(2));
        Destroy(transform.parent.gameObject, 0.5f);
    }

    public void Trap()
    {
        
    }
}
