using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorCageAoe : MonoBehaviour
{
    [SerializeField] private Transform cage;
    [SerializeField] private float     speed;
    [SerializeField] private float     lifespan;

    private Collider[] _colliders;
    private float      _progress;
    private float      _lifespanTimer;

    private void Awake()
    {
        _colliders = GetComponents<Collider>();
    }
    
    private void Update()
    {
        _lifespanTimer += Time.deltaTime;
        if (_lifespanTimer > lifespan)
            Destroy(gameObject);
        if (_progress      > 1) return;
        _progress += Time.deltaTime * speed;
        cage.localPosition = new Vector3(0, -1.95f + 2 * _progress, 0);
        if (_progress < 1) return;
        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<EffectSystem>().AddEffect(new StunEffect(2));
            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
        }
    }
}
