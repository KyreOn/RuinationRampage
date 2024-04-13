using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAttackEffect : Effect
{
    private float          _strength;
    

    public override float ApplyEffect()
    {
        return 1 / _strength;
    }

    public WeakAttackEffect(float strength) : base(-1)
    {
        effectId = 12;
        effectType = EffectType.SPEED;
        _strength = strength;
    }
}
