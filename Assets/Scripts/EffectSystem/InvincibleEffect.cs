using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityEffect : Effect
{
    public override float ApplyEffect()
    {
        return 1;
    }

    public InvincibilityEffect(float duration) : base(duration)
    {
        effectId = 0;
        effectType = EffectType.INVINCIBILITY;
    }
}
