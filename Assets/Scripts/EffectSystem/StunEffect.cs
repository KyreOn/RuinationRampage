using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : Effect
{
    public override float ApplyEffect()
    {
        return 1;
    }

    public StunEffect(float duration) : base(duration)
    {
        effectId = 3;
        effectType = EffectType.DISABLE;
    }
}
