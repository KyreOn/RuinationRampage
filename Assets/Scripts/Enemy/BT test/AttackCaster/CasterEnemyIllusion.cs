using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemyIllusion : CasterEnemy
{
    [SerializeField] private float lifespan;
    
    private float              _lifespanTimer;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _lifespanTimer += Time.deltaTime;
        if (_lifespanTimer >= lifespan)
            Destroy(gameObject);
    }
}
