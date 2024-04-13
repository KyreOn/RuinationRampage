using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorBlastAoe : MonoBehaviour
{
    [SerializeField] private float     lifeSpan;
    [SerializeField] private float     timeBetweenTicks;

    private float      _tickTimer;
    private float      _damage;
    private float      _slowPower;
    private Collider[] _player = new Collider[1];
    
    public void Init(float damage, float slowPower)
    {
        _damage = damage;
        _slowPower = slowPower;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
        _tickTimer -= Time.deltaTime;
        if (_tickTimer >= 0) return;
        if (Physics.OverlapSphereNonAlloc(transform.position, 2.5f, _player, 1 << 8) == 1)
            if (_player[0].gameObject.GetComponent<DamageSystem>().ApplyDamage(_damage))
                _player[0].gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(timeBetweenTicks,_slowPower), false);

        _tickTimer = timeBetweenTicks;
    }
}
