using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementEffect : Effect
{
    public Vector3 direction;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public DisplacementEffect(float duration, Vector3 direction) : base(duration)
    {
        effectId = 4;
        effectType = EffectType.DISPLACEMENT;
        this.direction = direction;
    }
}
