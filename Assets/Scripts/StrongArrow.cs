using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongArrow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    [SerializeField] private int   pierceCount;

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
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<DamageSystem>().ApplyDamage();
            other.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(1, 1.5f));
            pierceCount--;
        }
        else
            pierceCount = 0;

        if (pierceCount == 0)
            Destroy(gameObject);
    }
}
