using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float lifeSpan;

    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward);
    }
}
