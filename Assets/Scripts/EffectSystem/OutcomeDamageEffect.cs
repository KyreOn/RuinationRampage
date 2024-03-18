using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcomeDamageEffect : Effect
{
    private float          _strength;

    public override float ApplyEffect()
    {
        return _strength;
    }

    public OutcomeDamageEffect(float duration, float strength) : base(duration)
    {
        effectId = 9;
        effectType = EffectType.OUTCOMEDAMAGE;
        _strength = strength;
    }
}
