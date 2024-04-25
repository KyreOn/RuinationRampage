using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpellRAoe : MonoBehaviour
{
    [SerializeField] private float      lifeSpan;
    [SerializeField] private LayerMask  enemyLayer;
    [SerializeField] private float      timeBetweenTicks;
    [SerializeField] private GameObject aftershockEffect;
    
    private float _tickTimer;
    private float _damage;
    private float _slowPower;
    
    public void Init(float damage, float slowPower)
    {
        _damage = damage;
        _slowPower = slowPower;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            var aftershock = Instantiate(aftershockEffect, transform.position, Quaternion.identity);
            Destroy(aftershock, 1);
            Destroy(gameObject);
        }
            
        _tickTimer -= Time.deltaTime;
        if (_tickTimer >= 0) return;
        var enemiesInRange = Physics.OverlapSphere(transform.position, 2.5f, enemyLayer);
        foreach (var enemy in enemiesInRange)
        {
            if (enemy.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage))
                enemy.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(timeBetweenTicks,_slowPower), false);
        }

        _tickTimer = timeBetweenTicks;
    }
}
