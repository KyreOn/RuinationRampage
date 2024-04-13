using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorHookProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;

    private bool            _isReversed;
    private GameObject      _source;
    private DoctorHookSpell _spell;
    
    public void Init(GameObject source)
    {
        _source = source;
        _spell = source.GetComponent<DoctorHookSpell>();
    }
    
    private void Update()
    {
        if (transform.localPosition.magnitude > range)
        {
            _isReversed = true;
            GetComponent<Collider>().enabled = false;
        }

        if (_isReversed && transform.localPosition.magnitude <= 1)
        {
            Destroy(transform.parent.gameObject);
            _spell.OnEnd();
        }
            
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * ((_isReversed ? -1 : 1) * speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<DamageSystem>().ApplyDamage(10))
            {
                other.gameObject.GetComponent<EffectSystem>().AddEffect(new PullingEffect(_source.transform.position, 0.8f));
                _spell.isHit = true;
            }
                
            _isReversed = true;
        }
    }
}
