using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffect : Effect
{
    public override float ApplyEffect()
    {
        return 1;
    }

    public RootEffect(float duration) : base(duration)
    {
        effectId = 10;
        effectType = EffectType.ROOT;
    }
}
