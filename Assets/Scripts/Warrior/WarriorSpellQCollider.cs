using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WarriorSpellQCollider : MonoBehaviour
{
    private Collider _collider;
    private float    _timer;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
    
    public void Enable()
    {
        _collider.enabled = true;
        _timer = 0.15f;
    }

    public void Disable()
    {
        _collider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<DamageSystem>().ApplyDamage(10);
            other.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(_timer, transform.forward, 1));
            other.GetComponent<EffectSystem>().AddEffect(new StunEffect(1));
        }
    }

    private void Update()
    {
        _timer = Mathf.Clamp(_timer - Time.deltaTime, 0, 1);
    }
}
