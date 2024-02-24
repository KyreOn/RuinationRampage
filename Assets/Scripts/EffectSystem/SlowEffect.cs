using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : Effect
{
    private float          _strength;
    

    public override float ApplyEffect()
    {
        return 1 / _strength;
    }

    public SlowEffect(float duration, float strength) : base(duration)
    {
        effectId = 1;
        effectType = EffectType.SPEED;
        _strength = strength;
    }
}
