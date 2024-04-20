using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongArrow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;

    private GameObject _player;
    private float      _damage;
    private float      _bleedDuration;
    private float      _bleedDamage;
    private int        _pierceCount;
    
    public void Init(GameObject player, float damage, float bleedDuration, float bleedDamage, int pierceCount)
    {
        _player = player;
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
        var weakAttack   = _player.GetComponent<ArcherWeakAttack>();
        var strongAttack = _player.GetComponent<ArcherStrongAttack>();
        switch (other.gameObject.tag)
        {
            case "Reaction":
                weakAttack.OnHit();
                strongAttack.OnHit();
                other.gameObject.GetComponentInParent<Reaction>().TryReact(gameObject);
                return;
            case "Enemy":
            {
                if (_pierceCount > 0)
                {
                    weakAttack.OnHit();
                    strongAttack.OnHit();
                    if (other.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage))
                    {
                        other.gameObject.GetComponent<EffectSystem>().AddEffect(new StunEffect(0.2f));
                        other.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(1, 1.5f), false);
                        other.gameObject.GetComponent<EffectSystem>().AddEffect(new DOTEffect(_bleedDuration, 0.5f, _bleedDamage), false);
                    }
            
                    _pierceCount--;
                }
                break;
            }
            default:
                weakAttack.OnMiss();
                strongAttack.OnMiss();
                _pierceCount = 0;
                break;
        }

        if (_pierceCount == 0)
            Destroy(gameObject);
    }
}
