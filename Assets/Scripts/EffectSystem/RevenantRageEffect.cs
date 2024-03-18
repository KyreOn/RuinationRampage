using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantRageEffect : Effect
{
    private float          _strength;
    

    public override float ApplyEffect()
    {
        return 2;
    }

    public RevenantRageEffect(float duration) : base(duration)
    {
        effectId = 7;
        effectType = EffectType.SPEED;
    }
}
