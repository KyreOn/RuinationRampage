using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellEProjectile : MonoBehaviour
{
    [SerializeField] private float     speed;
    [SerializeField] private float     lifeSpan;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float     searchAngle;
    [SerializeField] private int       maxTargets;
    
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EffectSystem>().AddEffect(new StunEffect(3));
            maxTargets--;
            if (maxTargets > 0)
            {
                var nearEnemies = Physics.OverlapSphere(other.transform.position, 3, enemyLayer);
                foreach (var enemy in nearEnemies)
                {
                    if (maxTargets == 0) break;
                    if (enemy      == other) continue;

                    var direction = enemy.transform.position - other.transform.position;
                    var dot       = Vector3.Dot(transform.forward, direction.normalized);
                    if (dot < Mathf.Cos(Mathf.Deg2Rad * searchAngle)) continue;
                    enemy.GetComponent<EffectSystem>().AddEffect(new StunEffect(3));
                    maxTargets--;
                }
            }
        }
            
        Destroy(gameObject);
    }
}
