using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpellEProjectile : MonoBehaviour
{
    [SerializeField] private float        speed     = 15f;
    [SerializeField] private float        hitOffset = 0f;
    [SerializeField] private bool         UseFirePointRotation;
    [SerializeField] private Vector3      rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] private GameObject   hit;
    [SerializeField] private GameObject   flash;
    [SerializeField] private GameObject[] Detached;
    [SerializeField] private LayerMask    enemyLayer;
    [SerializeField] private float        searchAngle;
    [SerializeField] private GameObject   hitEffect;
    [SerializeField] private GameObject   chainEffect;
    [SerializeField] private Transform    model;

    private Rigidbody _rb;
    private float     _stunLength;
    private int       _maxTargets;
    private Vector3   _startPos;
    
    public void Init(float stunLength, int targets, Transform player)
    {
        _stunLength = stunLength;
        _maxTargets = targets;
        _startPos = player.position;
    }
    
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
        if (Vector3.Distance(transform.position, _startPos) > 10)
        {
            var hitInstance = Instantiate(hit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
            
        if (speed != 0)
        {
            _rb.velocity = transform.forward * speed;
            //transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    
        model.RotateAround(transform.position, transform.up, 20);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

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
        if (hit.tag == "Reaction")
        {
            hit.GetComponentInParent<Reaction>().TryReact(gameObject);
            return;
        }
        
        if (hit.CompareTag("Enemy"))
        {
            Debug.Log("hit");
            var hitEffect = Instantiate(chainEffect, transform.position, Quaternion.identity);
            hitEffect.GetComponent<ArcherEChain>().Init(hit.transform, hit.transform, _stunLength);
            hit.GetComponent<EffectSystem>().AddEffect(PlayerPrefs.GetString($"ChosenPerks0").Contains('5') ? new StunEffect(_stunLength) : new RootEffect(_stunLength));
            _maxTargets--;
            if (_maxTargets > 0)
            {
                var nearEnemies = Physics.OverlapSphere(hit.transform.position, PlayerPrefs.GetString($"ChosenPerks0").Contains('6') ? 5 : 4, enemyLayer);
                foreach (var enemy in nearEnemies)
                {
                    if (_maxTargets == 0) break;
                    if (enemy       == hit) continue;

                    var direction = enemy.transform.position - hit.transform.position;
                    var dot       = Vector3.Dot(transform.forward, direction.normalized);
                    if (dot < Mathf.Cos(Mathf.Deg2Rad * searchAngle)) continue;
                    enemy.GetComponent<EffectSystem>().AddEffect(PlayerPrefs.GetString($"ChosenPerks0").Contains('5') ? new StunEffect(_stunLength) : new RootEffect(_stunLength));
                    var effect = Instantiate(chainEffect, transform.position, Quaternion.LookRotation(direction));
                    effect.GetComponent<ArcherEChain>().Init(hit.transform, enemy.transform, _stunLength);
                    _maxTargets--;
                }
            }
        }
            
        Destroy(gameObject);
    }
}
