using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : Effect
{
    private float          _strength;

    public override float ApplyEffect()
    {
        return _strength;
    }

    public ShieldEffect(float strength) : base(-1)
    {
        effectId = 13;
        effectType = EffectType.INCOME_DAMAGE;
        _strength = strength;
    }
}
