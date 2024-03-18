using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementEffect : Effect
{
    public Vector3 direction;
    public float   speed;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public DisplacementEffect(float duration, Vector3 direction, float speed = 1) : base(duration)
    {
        effectId = 4;
        effectType = EffectType.DISPLACEMENT;
        this.direction = direction;
        this.speed = speed;
    }
}
