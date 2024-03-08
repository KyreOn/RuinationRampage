using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterSimpleAttackProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    
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
        if (other.gameObject.CompareTag("Player")) 
            other.gameObject.GetComponent<DamageSystem>().ApplyDamage();
        Destroy(gameObject);
    }
}
