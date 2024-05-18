using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorBurstAoe : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    
    private float      _scale;
    private Collider[] _player = new Collider[1];
    
    void Update()
    {
        _scale += Time.deltaTime * speed;
        //transform.localScale = new Vector3(_scale, 1, _scale);
        if (_scale > 3)
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, 1.5f, _player, 1 << 8) == 1)
            {
                if (_player[0].GetComponent<DamageSystem>().ApplyDamage(10))
                    _player[0].GetComponent<EffectSystem>().AddEffect(new SlowEffect(1, 1.5f));
            }
            Destroy(gameObject);
        }
    }
}
