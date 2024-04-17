using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WarriorSpellQCollider : MonoBehaviour
{
    private Collider _collider;
    private float    _timer;
    private float    _damage;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        transform.localScale = new Vector3(PlayerPrefs.GetString($"ChosenPerks1").Contains('4') ? 1.5f : 1, 1, 1);
    }
    
    public void Enable(float damage, float timer)
    {
        _collider.enabled = true;
        _damage = damage;
        _timer = timer;
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
            other.GetComponent<DamageSystem>().ApplyDamage(_damage);
            other.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(_timer, transform.forward, 1));
            other.GetComponent<EffectSystem>().AddEffect(new StunEffect((PlayerPrefs.GetString($"ChosenPerks1").Contains('5') ? 2 * (PlayerPrefs.GetString($"ChosenPerks1").Contains('1') ? 1.2f : 1) : 1)));
        }
    }

    private void Update()
    {
        _timer = Mathf.Clamp(_timer - Time.deltaTime, 0, 1);
    }
}
