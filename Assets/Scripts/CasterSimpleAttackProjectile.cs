using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterSimpleAttackProjectile : MonoBehaviour
{
    [SerializeField] private float        speed     = 15f;
    [SerializeField] private float        hitOffset = 0f;
    [SerializeField] private bool         UseFirePointRotation;
    [SerializeField] private Vector3      rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] private GameObject   hit;
    [SerializeField] private GameObject   flash;
    [SerializeField] private GameObject[] Detached;
    
    private                  Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }
    
    void FixedUpdate()
    {
        if (speed != 0)
        {
            _rb.velocity = transform.forward * speed;
            //transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        //rb.constraints = RigidbodyConstraints.FreezeAll;

        //GetComponent<ParticleSystem>().GetParticles(particles, 1);
        ContactPoint contact = collision.contacts[0];
        Quaternion   rot     = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3      pos     = contact.point + contact.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        OnHit(collision.gameObject);
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        //Destroy(gameObject);
    }
    
    private void OnHit(GameObject hit)
    {
        if (hit.CompareTag("Player")) 
            hit.GetComponent<DamageSystem>().ApplyDamage(10, transform);
        Destroy(gameObject);
    }
}
