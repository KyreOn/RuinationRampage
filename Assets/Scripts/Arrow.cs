using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float    speed;
    [SerializeField] private float    lifeSpan;

    private GameObject _player;
    private float _damage;

    public void Init(GameObject player, float damage)
    {
        _player = player;
        _damage = damage;
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
                other.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage);
                weakAttack.OnHit();
                strongAttack.OnHit();
                break;
            default:
                weakAttack.OnMiss();
                strongAttack.OnMiss();
                break;
        }

        Destroy(gameObject);
    }
}
