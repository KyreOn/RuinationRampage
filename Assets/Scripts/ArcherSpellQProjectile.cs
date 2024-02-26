using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellQProjectile : MonoBehaviour
{
    [SerializeField] private float     lifeSpan;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float     timeBetweenTicks;

    private float _tickTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
        _tickTimer -= Time.deltaTime;
        if (_tickTimer >= 0) return;
        var enemiesInRange = Physics.OverlapSphere(transform.position, 2, enemyLayer);
        foreach (var enemy in enemiesInRange)
        {
            enemy.gameObject.GetComponent<DamageSystem>().ApplyDamage();
            enemy.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(1,2));
        }

        _tickTimer = timeBetweenTicks;
    }
}
