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

    private float _stunLength;
    private int   _maxTargets;
    
    public void Init(float stunLength, int targets)
    {
        _stunLength = stunLength;
        _maxTargets = targets;
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
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EffectSystem>().AddEffect(PlayerPrefs.GetString($"ChosenPerks0").Contains('5') ? new StunEffect(_stunLength) : new RootEffect(_stunLength));
            _maxTargets--;
            if (_maxTargets > 0)
            {
                var nearEnemies = Physics.OverlapSphere(other.transform.position, PlayerPrefs.GetString($"ChosenPerks0").Contains('6') ? 4 : 3, enemyLayer);
                foreach (var enemy in nearEnemies)
                {
                    if (_maxTargets == 0) break;
                    if (enemy      == other) continue;

                    var direction = enemy.transform.position - other.transform.position;
                    var dot       = Vector3.Dot(transform.forward, direction.normalized);
                    if (dot < Mathf.Cos(Mathf.Deg2Rad * searchAngle)) continue;
                    enemy.GetComponent<EffectSystem>().AddEffect(PlayerPrefs.GetString($"ChosenPerks0").Contains('5') ? new StunEffect(_stunLength) : new RootEffect(_stunLength));
                    _maxTargets--;
                }
            }
        }
            
        Destroy(gameObject);
    }
}
