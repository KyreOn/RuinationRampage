using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeDamageEffect : Effect
{
    private float          _strength;

    public override float ApplyEffect()
    {
        return _strength;
    }

    public IncomeDamageEffect(float duration, float strength) : base(duration)
    {
        effectId = 8;
        effectType = EffectType.INCOMEDAMAGE;
        _strength = strength;
    }
}
