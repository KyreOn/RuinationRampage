using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : Effect
{
    private float _strength;
    

    public override float ApplyEffect()
    {
        return 1 / _strength;
    }

    public DamageEffect(float duration, float strength) : base(duration)
    {
        effectId = 0;
        effectType = EffectType.SPEED;
        _strength = strength;
    }
}
