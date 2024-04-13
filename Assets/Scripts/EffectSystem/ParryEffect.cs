using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryEffect : Effect
{
    private float _stunLength;
    public override float ApplyEffect()
    {
        return _stunLength;
    }

    public ParryEffect(float stunLength, float duration) : base(duration)
    {
        effectId = 14;
        effectType = EffectType.PARRY;
        _stunLength = stunLength;
    }
}
