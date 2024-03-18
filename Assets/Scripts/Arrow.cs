using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float    speed;
    [SerializeField] private float    lifeSpan;

    private float _damage;

    public void Init(float damage)
    {
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
        if (other.gameObject.tag == "Reaction")
        {
            other.gameObject.GetComponentInParent<Reaction>().TryReact(gameObject);
            return;
        }
        if (other.gameObject.tag == "Enemy") 
            other.gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage);
        Destroy(gameObject);
    }
}
