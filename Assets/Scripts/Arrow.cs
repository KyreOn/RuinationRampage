using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float    speed;
    [SerializeField] private float    lifeSpan;

    private GameObject _player;
    private float      _damage;
    private int        _pierceCount;

    public void Init(GameObject player, float damage)
    {
        _player = player;
        _damage = damage;
        _pierceCount = 1;
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
                other.gameObject.GetComponentInParent<Reaction>().TryReact(gameObject);
                weakAttack.OnHit();
                strongAttack.OnHit();
                return;
            case "Enemy":
                if (_pierceCount > 0)
                {
                    other.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage);
                    weakAttack.OnHit();
                    strongAttack.OnHit();
                    _pierceCount--;
                }
                break;
            default:
                weakAttack.OnMiss();
                strongAttack.OnMiss();
                break;
        }

        Destroy(gameObject);
    }
}
