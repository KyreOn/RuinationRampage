using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingEffect : Effect
{
    public Vector3 target;
    public float   speed;
    
    public PullingEffect(Vector3 target, float speed) : base(-1)
    {
        effectId = 16;
        effectType = EffectType.PULLING;
        this.target = target;
        this.speed = speed;
    }
}
