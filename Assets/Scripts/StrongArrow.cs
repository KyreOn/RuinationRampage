using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongArrow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;

    private float _damage;
    private float _bleedDuration;
    private float _bleedDamage;
    private int   _pierceCount;
    
    public void Init(float damage, float bleedDuration, float bleedDamage, int pierceCount)
    {
        _damage = damage;
        _bleedDuration = bleedDuration;
        _bleedDamage = bleedDamage;
        _pierceCount = pierceCount;
    }
    
    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reaction")
        {
            other.gameObject.GetComponentInParent<Reaction>().TryReact(gameObject);
            return;
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage))
            {
                other.gameObject.GetComponent<EffectSystem>().AddEffect(new StunEffect(0.2f));
                other.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(1, 1.5f),    false);
                other.gameObject.GetComponent<EffectSystem>().AddEffect(new DOTEffect(_bleedDuration, 0.5f, _bleedDamage), false);
            }
            
            _pierceCount--;
        }
        else
            _pierceCount = 0;

        if (_pierceCount == 0)
            Destroy(gameObject);
    }
}
