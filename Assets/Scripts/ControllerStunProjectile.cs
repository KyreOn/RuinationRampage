using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStunProjectile : MonoBehaviour
{
    [SerializeField] private float      speed;
    [SerializeField] private float      lifeSpan;
    [SerializeField] private GameObject onHitSpell;
    
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
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            var damageSystem = other.gameObject.GetComponent<DamageSystem>();
            if (damageSystem.isInvincible) return;
            damageSystem.ApplyDamage();
            other.gameObject.GetComponent<EffectSystem>().AddEffect(new StunEffect(1));
            var pos = other.transform.position;
            pos.y = 0.1f;
            Instantiate(onHitSpell, pos, Quaternion.identity);
        }
            
        Destroy(gameObject);
    }
}
